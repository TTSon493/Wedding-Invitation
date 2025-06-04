using System.Security.Claims;
using AutoMapper;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.Utility.Constants;

namespace Wedding.Service.Service;

public class BalanceService : IBalanceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStripeService _stripeService;
    private readonly ITransactionService _transactionService;

    public BalanceService(IUnitOfWork unitOfWork, IMapper mapper, IStripeService stripeService,
        ITransactionService transactionService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _stripeService = stripeService;
        _transactionService = transactionService;
    }

    public async Task<ResponseDTO> GetSystemBalance(ClaimsPrincipal User)
    {
        try
        {
            var responseDto = await _stripeService.GetStripeBalance();
            var result = (Stripe.Balance)responseDto.Result!;
            return new ResponseDTO()
            {
                Result = new
                {
                    AvailableBalance = result.Available,
                    PendingBalance = result.Pending,
                    ConnectReserved = result.ConnectReserved
                }
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ResponseDTO> GetCustomerBalance(ClaimsPrincipal User, string? userId)
    {
        try
        {
            var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)!.Value;
            var balance = new Balance();

            if (role.Contains(StaticUserRoles.Admin))
            {
                if (userId is null)
                {
                    return new ResponseDTO()
                    {
                        IsSuccess = false,
                        Message = "User was not found",
                        StatusCode = 404,
                        Result = null
                    };
                }

                balance = await _unitOfWork.BalanceRepository.GetAsync(x => x.UserId == userId);
            }

            if (role.Contains(StaticUserRoles.Customer))
            {
                userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
                balance = await _unitOfWork.BalanceRepository.GetAsync(x => x.UserId == userId);
            }

            var getBalanceDto = _mapper.Map<GetBalanceDTO>(balance);

            return new ResponseDTO()
            {
                IsSuccess = true,
                Message = "Get balance successfully",
                StatusCode = 200,
                Result = getBalanceDto
            };
        }
        catch (Exception e)
        {
            return new ResponseDTO()
            {
                IsSuccess = false,
                Message = e.Message,
                Result = null,
                StatusCode = 500
            };
        }
    }

    public async Task<ResponseDTO> UpsertBalance(UpsertBalanceDTO upsertBalanceDto)
    {
        try
        {
            var balance = await _unitOfWork.BalanceRepository.GetAsync(x => x.UserId == upsertBalanceDto.UserId);
            if (balance is null)
            {
                balance = new Balance()
                {
                    Currency = "usd",
                    UserId = upsertBalanceDto.UserId,
                    TotalBalance = 0,
                    AvailableBalance = upsertBalanceDto.AvailableBalance,
                    PayoutBalance = upsertBalanceDto.PayoutBalance,
                    UpdatedTime = DateTime.UtcNow
                };

                await _unitOfWork.BalanceRepository.AddAsync(balance);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO()
                {
                    IsSuccess = true,
                    Message = "Upsert balance successfully",
                    Result = null,
                    StatusCode = 200
                };
            }

            balance.AvailableBalance += upsertBalanceDto.AvailableBalance;
            balance.PayoutBalance += upsertBalanceDto.PayoutBalance;
            balance.UpdatedTime = DateTime.UtcNow;
            balance.TotalBalance = balance.AvailableBalance + balance.PayoutBalance;

            _unitOfWork.BalanceRepository.Update(balance);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                IsSuccess = true,
                Message = "Upsert balance successfully",
                Result = null,
                StatusCode = 200
            };
        }
        catch (Exception e)
        {
            return new ResponseDTO()
            {
                IsSuccess = false,
                Message = e.Message,
                Result = null,
                StatusCode = 500
            };
        }
    }

    // public Task<ResponseDTO> UpdateAvailableBalanceByOrderId(Guid orderHeaderId)
    // {
    //     throw new NotImplementedException();
    // }

    public async Task<ResponseDTO> UpdateAvailableBalanceByOrderId(Guid orderHeaderId)
    {
        try
        {
            var ordersDetails =
                await _unitOfWork.OrderDetailsRepository.GetAllAsync(x => x.OrderHeaderId == orderHeaderId);

            foreach (var orderDetails in ordersDetails)
            {
                var card = await _unitOfWork.CardRepository.GetAsync
                (
                    x => x.Id == orderDetails.CardId
                );

                var customer = await _unitOfWork.CustomerRepository.GetAsync
                (
                    x => x.CustomerId == card!.CustomerId
                );

                await _stripeService.CreateTransfer
                (
                    new CreateStripeTransferDTO()
                    {
                        Currency = "usd",
                        UserId = customer!.UserId,
                        Amount = (long)(orderDetails.CardPrice),
                        ConnectedAccountId = customer.StripeAccountId
                    }
                );

                await UpsertBalance(
                    new UpsertBalanceDTO()
                    {
                        Currency = "usd",
                        AvailableBalance = orderDetails.CardPrice,
                        PayoutBalance = 0,
                        UserId = customer.UserId
                    }
                );

                await _transactionService.CreateTransaction
                (
                    new CreateTransactionDTO()
                    {
                        UserId = customer.UserId,
                        Amount = orderDetails.CardPrice,
                        Type = StaticEnum.TransactionType.Income,
                    }
                );
            }

            return new ResponseDTO()
            {
                Message = "Update balance successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = null
            };
        }
        catch (Exception e)
        {
            return new ResponseDTO()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = 500,
                Result = null
            };
        }
    }
}
using System.Security.Claims;
using AutoMapper;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;
using Wedding.Model.DTO;
using Wedding.Service.IService;

namespace Wedding.Service.Service;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CartService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDTO> GetCart(ClaimsPrincipal User)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var customer = await _unitOfWork.CustomerRepository.GetAsync(x => x.UserId == userId);

            if (customer is null)
            {
                return new ResponseDTO()
                {
                    Message = "Customer was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var cartHeader = await _unitOfWork.CartHeaderRepository.GetAsync(x => x.CustomerId == customer.CustomerId);

            if (cartHeader is null)
            {
                cartHeader = new CartHeader()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.CustomerId,
                    TotalPrice = 0
                };
                await _unitOfWork.CartHeaderRepository.AddAsync(cartHeader);
                await _unitOfWork.SaveAsync();
            }

            var cartsDetails =
                await _unitOfWork.CartDetailsRepository.GetAllAsync(x => x.CartHeaderId == cartHeader.Id);

            var cartHeaderDto = _mapper.Map<CartHeaderDTO>(cartHeader);
            var cartDetailsDto = _mapper.Map<IEnumerable<CartDetailsDTO>>(cartsDetails);
            cartHeaderDto.CartDetailsDtos = cartDetailsDto;

            return new ResponseDTO()
            {
                Message = "Get cart successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = cartHeaderDto
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

    public async Task<ResponseDTO> AddToCart(ClaimsPrincipal User, AddToCartDTO addToCartDto)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var customer = await _unitOfWork.CustomerRepository.GetAsync(x => x.UserId == userId);
            if (customer is null)
            {
                return new ResponseDTO()
                {
                    Message = "Customer was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var card = await _unitOfWork.CardRepository.GetAsync(x => x.Id == addToCartDto.CardId);
            if (card is null)
            {
                return new ResponseDTO()
                {
                    Message = "Card was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var cartHeader = await _unitOfWork.CartHeaderRepository.GetAsync(x => x.CustomerId == customer.CustomerId);
            if (cartHeader is null)
            {
                cartHeader = new CartHeader()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.CustomerId,
                    TotalPrice = 0
                };
                await _unitOfWork.CartHeaderRepository.AddAsync(cartHeader);
                await _unitOfWork.SaveAsync();
            }

            var cartDetails =
                await _unitOfWork.CartDetailsRepository.GetAsync
                (
                    filter: x => x.CartHeaderId == cartHeader.Id && x.CardId == card.Id
                );

            if (cartDetails is not null)
            {
                return new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = null,
                    StatusCode = 200,
                    Message = "Your card already exist in cart"
                };
            }

            cartDetails = new CartDetails()
            {
                Id = Guid.NewGuid(),
                CartHeaderId = cartHeader.Id,
                CardId = card.Id,
                CardPrice = cartDetails.CardPrice,
                CardTitle = cartDetails.CardTitle
            };

            cartHeader.TotalPrice += cartDetails.CardPrice;
            _unitOfWork.CartHeaderRepository.Update(cartHeader);

            await _unitOfWork.CartDetailsRepository.AddAsync(cartDetails);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Add card to cart successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = addToCartDto
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

    public async Task<ResponseDTO> RemoveFromCart(ClaimsPrincipal User, Guid cardId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var customer = await _unitOfWork.CustomerRepository.GetAsync
            (
                filter: x => x.UserId == userId
            );
            if (customer is null)
            {
                return new ResponseDTO()
                {
                    Message = "Customer was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var cartHeader = await _unitOfWork.CartHeaderRepository.GetAsync
            (
                filter: x => x.CustomerId == customer.CustomerId
            );
            if (cartHeader is null)
            {
                cartHeader = new CartHeader()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.CustomerId,
                    TotalPrice = 0
                };
                await _unitOfWork.CartHeaderRepository.AddAsync(cartHeader);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO()
                {
                    Message = "Your cart is empty",
                    StatusCode = 404,
                    Result = null,
                    IsSuccess = false
                };
            }

            var cartDetails = await _unitOfWork.CartDetailsRepository.GetAsync
            (
                filter: x => x.CardId == cardId && x.CartHeaderId == cartHeader.Id
            );
            if (cartDetails is null)
            {
                return new ResponseDTO()
                {
                    Message = "Your card does not exist in cart",
                    StatusCode = 404,
                    Result = null,
                    IsSuccess = false
                };
            }

            //cartHeader.TotalPrice -= cartDetails.CoursePrice;
            _unitOfWork.CartHeaderRepository.Update(cartHeader);

            _unitOfWork.CartDetailsRepository.Remove(cartDetails);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Remove card in cart successfully",
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

    public Task<ResponseDTO> Checkout(ClaimsPrincipal User)
    {
        throw new NotImplementedException();
    }
}
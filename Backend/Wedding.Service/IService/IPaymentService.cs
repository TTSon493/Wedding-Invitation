using System.Security.Claims;
using Wedding.Model.DTO;

namespace Wedding.Service.IService;

public interface IPaymentService
{
    Task<ResponseDTO> CreateStripeConnectedAccount
    (
        ClaimsPrincipal User,
        CreateStripeConnectedAccountDTO createStripeConnectedAccountDto
    );

    Task<ResponseDTO> CreateStripeTransfer(CreateStripeTransferDTO createStripeTransferDto);
    Task<ResponseDTO> AddStripeCard(AddStripeCardDTO addStripeCardDto);
    Task<ResponseDTO> CreateStripePayout(ClaimsPrincipal User, CreateStripePayoutDTO createStripePayoutDto);
    Task<ResponseDTO> GetLeastCustomersByPayout(int topN, int? filterYear, int? filterMonth, int? filterQuarter);
    Task<ResponseDTO> GetTopCustomersByPayout(int topN, int? filterYear, int? filterMonth, int? filterQuarter);
}
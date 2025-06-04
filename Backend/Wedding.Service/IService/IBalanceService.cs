using System.Security.Claims;
using Wedding.Model.DTO;

namespace Wedding.Service.IService;

public interface IBalanceService
{
    Task<ResponseDTO> GetSystemBalance(ClaimsPrincipal User);
    Task<ResponseDTO> GetCustomerBalance(ClaimsPrincipal User, string? userId);
    Task<ResponseDTO> UpsertBalance(UpsertBalanceDTO upsertBalanceDto);
    Task<ResponseDTO> UpdateAvailableBalanceByOrderId(Guid orderHeaderId);
}
using System.Security.Claims;
using Wedding.Model.DTO;

namespace Wedding.Service.IService;

public interface IOrderStatusService
{
    Task<ResponseDTO> GetOrdersStatus(Guid orderHeaderId);
    Task<ResponseDTO> GetOrderStatus(Guid orderStatusId);
    Task<ResponseDTO> CreateOrderStatus(ClaimsPrincipal User, CreateOrderStatusDTO createOrderStatusDto);
}
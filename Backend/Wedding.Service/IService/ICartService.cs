using System.Security.Claims;
using Wedding.Model.DTO;

namespace Wedding.Service.IService;

public interface ICartService
{
    Task<ResponseDTO> GetCart(ClaimsPrincipal User);
    Task<ResponseDTO> AddToCart(ClaimsPrincipal User, AddToCartDTO addToCartDto);
    Task<ResponseDTO> RemoveFromCart(ClaimsPrincipal User, Guid cardId);
    Task<ResponseDTO> Checkout(ClaimsPrincipal User);
}
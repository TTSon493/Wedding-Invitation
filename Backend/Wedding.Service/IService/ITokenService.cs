using System.Security.Claims;
using Wedding.Model.Domain;
using Wedding.Model.Domain;

namespace Wedding.Service.IService;

public interface ITokenService
{
    Task<string> GenerateJwtAccessTokenAsync(ApplicationUser user);
    Task<string> GenerateJwtRefreshTokenAsync(ApplicationUser user);
    Task<bool> StoreRefreshToken(string userId, string refreshToken);
    Task<ClaimsPrincipal> GetPrincipalFromToken(string token);
    Task<string> RetrieveRefreshToken(string userId);
    Task<bool> DeleteRefreshToken(string userId);

}

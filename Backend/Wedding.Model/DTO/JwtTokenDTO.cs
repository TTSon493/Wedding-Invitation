using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class JwtTokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

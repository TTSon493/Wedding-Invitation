using System.Text.Json.Serialization;

namespace Wedding.Model.DTO
{
    public class SignByGoogleResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Boolean IsProfileComplete { get; set; }
    }
}

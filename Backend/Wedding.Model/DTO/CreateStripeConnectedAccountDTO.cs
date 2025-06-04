using System.Text.Json.Serialization;
using Wedding.Utility.Constants;

namespace Wedding.Model.DTO;

public class CreateStripeConnectedAccountDTO
{
    public string RefreshUrl { get; set; }
    public string ReturnUrl { get; set; }
    [JsonIgnore] public string? Email { get; set; }
}
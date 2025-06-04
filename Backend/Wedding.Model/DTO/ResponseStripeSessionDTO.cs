using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class ResponseStripeSessionDTO
{
    public string? StripeSessionId { get; set; }
    public string? StripeSessionUrl { get; set; }
}
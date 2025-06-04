using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class ResponseStripeConnectedAccountDTO
{
    public string? AccountId { get; set; }
    public string? AccountLinkUrl { get; set; }
}
using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class CreateStripePayoutDTO
{
    [JsonIgnore]
    public string? ConnectedAccountId { get; set; }
    public long Amount { get; set; }
    public string Currency { get; set; }
}
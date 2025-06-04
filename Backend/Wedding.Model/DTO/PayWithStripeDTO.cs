using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class PayWithStripeDTO
{
    public string ApprovedUrl { get; set; }
    public string CancelUrl { get; set; }
    public Guid OrderHeaderId { get; set; }
}
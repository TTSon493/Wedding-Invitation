using System.Text.Json.Serialization;
using Wedding.Model.Domain;

namespace Wedding.Model.DTO;

public class CreateStripeSessionDTO
{
    public IEnumerable<OrderDetails>? OrdersDetails { get; set; }
    public string? ApprovedUrl { get; set; }
    public string? CancelUrl { get; set; }
}
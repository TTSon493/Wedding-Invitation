using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class ValidateWithStripeDTO
{
    public Guid OrderHeaderId { get; set; }
}
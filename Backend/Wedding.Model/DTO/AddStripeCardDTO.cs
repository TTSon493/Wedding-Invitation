using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class AddStripeCardDTO
{
    public string ConnectedAccountId { get; set; }
    public string CardToken { get; set; }
}
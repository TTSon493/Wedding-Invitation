using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class GetOrderDetailsDTO
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public string? CardTitle { get; set; }
    public double CardPrice { get; set; }
    public Guid OrderHeaderId { get; set; }
}
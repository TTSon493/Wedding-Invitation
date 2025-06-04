using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class CartDetailsDTO
{
    public Guid Id { get; set; }
    public Guid CartHeaderId { get; set; }
    public Guid CardId { get; set; }
    public double CardPrice { get; set; }
    public string? CardTitle { get; set; }
}
using System.Text.Json.Serialization;
using Wedding.Model.DTO;

namespace Wedding.Model.Domain;

public class CartHeaderDTO
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public double TotalPrice { get; set; }

    public IEnumerable<CartDetailsDTO> CartDetailsDtos { get; set; }
}
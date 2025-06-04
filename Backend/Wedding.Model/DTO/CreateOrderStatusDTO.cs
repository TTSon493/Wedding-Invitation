using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class CreateOrderStatusDTO
{
    public Guid OrderHeaderId { get; set; }
    public int Status { get; set; }
}
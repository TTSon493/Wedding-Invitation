using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class CustomerInfoDTO
{
    public Guid? CustomerId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}

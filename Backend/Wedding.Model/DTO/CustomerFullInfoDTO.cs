using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class CustomerFullInfoDTO
{
    public Guid? CustomerId { get; set; }
    public string UserId { get; set; }
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class UpdateCustomerProfileDTO
{
    [Required]
    public string? Gender { get; set; }

    [Required]
    public string? FullName { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime? BirthDate { get; set; }

    [Required]
    public string? Country {  get; set; }

    [Required]
    public string? Address { get; set; }

    [Required]
    public string? CardNumber { get; set; }

    [Required]
    public string? CardName { get; set; }

    [Required]
    public string? CardProvider { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Wedding.Model.DTO
{
    public class UpdateCustomerDTO
    {
        [Required] public Guid CustomerId { get; set; }
        [Required] public string? Gender { get; set; }
        [Required] public string? FullName { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }
        [Required] public string? Country { get; set; }
        [Required] public string? Address { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class ForgotPasswordDTO
{
    [Required(ErrorMessage = "Please enter email or phone number")]
    public string EmailOrPhone { get; set; }
}

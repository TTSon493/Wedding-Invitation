using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class SendVerifyEmailDTO
{
    [EmailAddress]
    public string Email { get; set; }
}
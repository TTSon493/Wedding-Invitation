using System.ComponentModel.DataAnnotations;
using Wedding.Utility.ValidationAttribute;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class AvatarUploadDTO
{
    [Required]
    [MaxFileSize(1)]
    [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
    public IFormFile File { get; set; }
}
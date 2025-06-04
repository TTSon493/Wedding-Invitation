using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Wedding.Utility.ValidationAttribute;

namespace Wedding.Model.DTO;

public class UploadEventPhotoBackgroundImg
{
    [Required]
    [MaxFileSize(10)]
    [AllowedExtensions(new string[] { ".img", ".png", ".jpg" })]
    public List<IFormFile> File { get; set; }
}
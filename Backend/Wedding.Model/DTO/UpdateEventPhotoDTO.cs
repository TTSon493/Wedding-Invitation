using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class UpdateEventPhotoDTO
{
    public string[] PhotoUrl { get; set; }
    public string PhotoType { get; set; }
    public DateTime UploadedDate { get; set; }
}

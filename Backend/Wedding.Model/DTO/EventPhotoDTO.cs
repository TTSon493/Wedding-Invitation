using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class EventPhotoDTO
{
    public Guid EventPhotoId { get; set; }
    public Guid EventId { get; set; }
    public string[] PhotoUrl { get; set; }
    public string PhotoType { get; set; }
    public DateTime UploadedDate { get; set; }
}

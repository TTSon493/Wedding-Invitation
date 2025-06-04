using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class UpdateEventDTO
{
    public DateTime EventDate { get; set; }
    public string EventLocation { get; set; }
    public string[] EventPhotoUrl { get; set; }
    public DateTime CreatedDate { get; set; }
}

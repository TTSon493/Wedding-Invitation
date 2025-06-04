using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class EventDTO
{
    public Guid EventId { get; set; }
    public Guid WeddingId { get; set; }
    public string BrideName { get; set; }
    public string GroomName { get; set; }
    public DateTime EventDate { get; set; }
    public string EventLocation { get; set; }
    public string[] EventPhotoUrl { get; set; }
    public DateTime CreatedDate { get; set; }
}

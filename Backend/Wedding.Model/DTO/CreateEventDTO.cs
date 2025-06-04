using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Wedding.Model.Domain;

namespace Wedding.Model.DTO;

public class CreateEventDTO
{
    public Guid EventId { get; set; }
    public Guid WeddingId { get; set; }
    public DateTime EventDate { get; set; }
    public string EventLocation { get; set; }
    public string[] EventPhotoUrl { get; set; }
    public DateTime CreatedDate { get; set; }
}

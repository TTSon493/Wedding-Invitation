using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;

public class Event
{
    [Key]
    public Guid EventId { get; set; }
    public Guid WeddingId { get; set; }
    [ForeignKey("WeddingId")] public virtual Wedding Wedding { get; set; }
    public string BrideName { get; set; }  
    public string GroomName { get; set; }
    public DateTime EventDate { get; set; }
    public string EventLocation { get; set; }
    public string[] EventPhotoUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public virtual ICollection<EventPhoto> EventPhotos { get; set; }
}

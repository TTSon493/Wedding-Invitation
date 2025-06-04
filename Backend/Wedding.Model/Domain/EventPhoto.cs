using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;

public  class EventPhoto
{
    [Key]
    public Guid EventPhotoId { get; set; }
    public Guid? EventId { get; set; }
    [ForeignKey("EventId")] public virtual Event Event { get; set; }
    public string[] PhotoUrl { get; set; }
    public string PhotoType { get; set; }
    
    public DateTime UploadedDate { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;

public class Wedding
{
    [Key]
    public Guid WeddingId { get; set; }
    public string UserId { get; set; }
    [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; }
    public string BrideName { get; set; }
    public string GroomName { get; set; }
    public DateTime WeddingDate { get; set; }
    public string WeddingLocation { get; set; }
    public string[] WeddingPhotoUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public virtual ICollection<Event> Events { get; set; }
    public virtual ICollection<Invitation> Invitations { get; set; }
}
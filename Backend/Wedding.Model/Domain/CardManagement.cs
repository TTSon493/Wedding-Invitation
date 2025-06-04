using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Wedding.Model.Domain;

public class CardManagement
{
    [Key]
    public Guid CardId { get; set; }
    public string UserId { get; set; }
    [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; }
    public Guid InvitationId { get; set; }
    [ForeignKey("InvitationId"), DeleteBehavior(DeleteBehavior.NoAction)] public virtual Invitation Invitation { get; set; }
    public string AttendStatus { get; set; }
    public DateTime CreatedTime { get; set; }
    public virtual ICollection<Wedding> Weddings { get; set; }
}
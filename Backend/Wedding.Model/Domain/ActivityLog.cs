using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;

public class ActivityLog
{
    [Key] public Guid LogId { get; set; }
    public string UserId { get; set; } 
    [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; }
    public string Action { get; set; }
    public string Entity { get; set; }
    public string TimeStamp { get; set; }
}
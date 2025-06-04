using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;

public class Invitation
{
    [Key]
    public Guid InvitationId { get; set; }
    public Guid? WeddingId { get; set; }
    [ForeignKey("WeddingId")]
    public virtual Wedding Wedding { get; set; }
    public Guid? TemplateId { get; set; }
    [ForeignKey("TemplateId")] 
    public virtual InvitationTemplate? InvitationTemplate { get; set; }
    public DateTime InvationLocation { get; set; }
    public string[] InvitationPhotoUrl { get; set; }
    public string CustomerMessage { get; set; }
    public string CustomerTextColor { get; set; }
    public string ShareableLink { get; set; }
    public DateTime CreatedTime { get; set; }
    
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;

public class InvitationTemplate
{
    [Key]
    public Guid TemplateId { get; set; }
    public string TemplateName { get; set; }
    public string[] BackgroundImageUrl { get; set; }
    public string[] TextColor { get; set; }
    public string[] TextFont { get; set; }
    public string Description { get; set; }
    public DateTime CreatedBy { get; set; }
    public Guid? InvitationId { get; set; }
    [ForeignKey("InvitationId")] 
    public virtual Invitation? Invitation { get; set; }
}
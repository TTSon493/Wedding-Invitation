using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;

public class InvitationHtml
{
    [Key]
    public Guid HtmlId { get; set; }
    public Guid? InvitationId { get; set; }
    [ForeignKey("InvitationId")]
    public virtual Invitation? Invitation { get; set; }
    public string HtmlContent { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdateddTime { get; set; }
}
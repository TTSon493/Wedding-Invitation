using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class InvitationTemplateDTO
{
    public Guid TemplateId { get; set; }
    public string TemplateName { get; set; }
    public string[] BackgroundImageUrl { get; set; }
    public string[] TextColor { get; set; }
    public string[] TextFont { get; set; }
    public string Description { get; set; }
    public DateTime CreatedBy { get; set; }
    public Guid? InvitationId { get; set; }
}
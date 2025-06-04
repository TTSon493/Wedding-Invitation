using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class InvitationDTO
{
    public Guid InvitationId { get; set; }
    public Guid? WeddingId { get; set; }
    public Guid? TemplateId { get; set; }
    public string CustomerMessage { get; set; }
    public string CustomerTextColor { get; set; }
    public string ShareableLink { get; set; }
    public DateTime CreatedTime { get; set; }
}
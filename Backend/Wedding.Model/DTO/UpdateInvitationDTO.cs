using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class UpdateInvitationDTO
{
    public Guid WeddingId { get; set; }
    public string TemplateId { get; set; }
    public DateTime InvationLocation { get; set; }
    public string[] InvitationPhotoUrl { get; set; }
    public string CustomerMessage { get; set; }
    public string CustomerTextColor { get; set; }
    public string ShareableLink { get; set; }
    public DateTime CreatedTime { get; set; }
}
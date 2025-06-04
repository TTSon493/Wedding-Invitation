namespace Wedding.Model.DTO;

public class CreateInvitationHtmlDTO
{
    public Guid HtmlId { get; set; }
    public Guid? InvitationId { get; set; }
    public string HtmlContent { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdateddTime { get; set; }
}
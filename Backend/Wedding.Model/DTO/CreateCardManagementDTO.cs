using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class CreateCardManagementDTO
{
    public Guid CardId { get; set; }
    public string UserId { get; set; }
    public Guid InvitationId { get; set; }
    public string AttendStatus { get; set; }
    public DateTime CreatedTime { get; set; }
}
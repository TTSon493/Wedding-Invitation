using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class UpdateCardManagementDTO
{
    public Guid CardId { get; set; }
    public Guid InvitationId { get; set; }
    public string AttendStatus { get; set; }
    public DateTime CreatedTime { get; set; }
}
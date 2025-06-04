using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class UpdateActivityLogDTO
{
    public Guid LogId { get; set; }
    public string Action { get; set; }
    public string Entity { get; set; }
    public string TimeStamp { get; set; }
}
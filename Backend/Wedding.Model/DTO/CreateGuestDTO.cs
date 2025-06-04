using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class CreateGuestDTO
{
    public Guid GuestId { get; set; }
    public Guid? GuestListId { get; set; }
    public Guid EventId { get; set; }
    public string Name { get; set; }
    public string Attend { get; set; }
    public string Gift { get; set; }
}

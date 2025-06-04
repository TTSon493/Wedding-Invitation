using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Wedding.Model.Domain;
using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class CreateGuestListDTO
{
    public Guid GuestListId { get; set; }
    public Guid GuestId { get; set; }
    public Guid EventId { get; set; }
    public string GuestName { get; set; }
    public string AttendStatus { get; set; }
    public DateTime CheckinTime { get; set; }
    public string GuestGift { get; set; }
}


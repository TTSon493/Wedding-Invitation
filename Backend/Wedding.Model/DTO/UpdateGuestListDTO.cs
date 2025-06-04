using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Wedding.Model.Domain;

namespace Wedding.Model.DTO;

public class UpdateGuestListDTO
{
    public string GuestName { get; set; }
    public string AttendStatus { get; set; }
    public DateTime CheckinTime { get; set; }
    public string GuestGift { get; set; }
}

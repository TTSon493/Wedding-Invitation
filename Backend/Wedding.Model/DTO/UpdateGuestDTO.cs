using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class UpdateGuestDTO
{
    public string Name { get; set; }
    public string Attend { get; set; }
    public string Gift { get; set; }
}

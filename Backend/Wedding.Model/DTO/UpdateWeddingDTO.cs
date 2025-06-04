using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class UpdateWeddingDTO
{
    public string BrideName { get; set; }
    public string GroomName { get; set; }
    public DateTime WeddingDate { get; set; }
    public string WeddingLocation { get; set; }
    public string[] WeddingPhotoUrl { get; set; }
    public DateTime CreatedDate { get; set; }
}
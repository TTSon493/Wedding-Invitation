using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class CreateWeddingDTO
{
    public Guid WeddingId { get; set; }
    public string UserId { get; set; }
    public string BrideName { get; set; }
    public string GroomName { get; set; }
    public DateTime WeddingDate { get; set; }
    public string WeddingLocation { get; set; }
    public string[] WeddingPhotoUrl { get; set; }
    public DateTime CreatedDate { get; set; }
}
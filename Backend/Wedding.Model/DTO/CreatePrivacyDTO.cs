using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class CreatePrivacyDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
}
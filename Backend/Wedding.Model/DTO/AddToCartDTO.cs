using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class AddToCartDTO
{
    [Required] public Guid CardId { get; set; }
}
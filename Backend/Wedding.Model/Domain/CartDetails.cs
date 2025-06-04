using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;

public class CartDetails
{
    [Key] public Guid Id { get; set; }

    public Guid CartHeaderId { get; set; }
    [ForeignKey("CartHeaderId")] public virtual CartHeader CartHeader { get; set; }

    public Guid CardId { get; set; }
    public string? CardTitle { get; set; }
    public double CardPrice { get; set; }
}
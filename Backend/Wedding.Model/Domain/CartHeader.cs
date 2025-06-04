using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;

public class CartHeader
{
    [Key] public Guid Id { get; set; }

    public Guid CustomerId { get; set; }
    [ForeignKey("CustomerId")] public virtual Customer Customer { get; set; }

    public double TotalPrice { get; set; }

    [NotMapped] public virtual IEnumerable<CartDetails> CartDetails { get; set; }
}
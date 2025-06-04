using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wedding.Model.Domain;

namespace Wedding.Model.Domain;

public class OrderHeader : BaseEntity<string, string, int>
{
    [Key] public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    [ForeignKey("CustomerId")] public virtual Customer Customer { get; set; }
    public double OrderPrice { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? StripeSessionId { get; set; }
    [NotMapped] public virtual IEnumerable<OrderDetails> OrderDetails { get; set; }
}
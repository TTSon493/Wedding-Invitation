using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;

public class OrderDetails
{
    [Key] public Guid Id { get; set; }

    public Guid CardId { get; set; }
    public string? CardTitle { get; set; }
    public double CardPrice { get; set; }

    public Guid OrderHeaderId { get; set; }
    [ForeignKey("OrderHeaderId")] public virtual OrderHeader OrderHeader { get; set; }
}
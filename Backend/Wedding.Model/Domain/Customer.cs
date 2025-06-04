using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;
public class Customer
{
    [Key]
    public Guid CustomerId { get; set; }
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual ApplicationUser ApplicationUser { get; set; }
    public string? StripeAccountId { get; set; }
    public bool? IsAccepted { get; set; } = false;
    public DateTime? AcceptedTime { get; set; }
    public string? AcceptedBy { get; set; }
    public DateTime? RejectedTime { get; set; }
    public string? RejectedBy { get; set; }
}

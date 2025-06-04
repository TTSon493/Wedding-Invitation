using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wedding.Utility.Constants;
using Microsoft.ApplicationInsights;

namespace Wedding.Model.Domain;

public class Transaction
{
    [Key] public Guid Id { get; set; }
    public string UserId { get; set; }
    public StaticEnum.TransactionType Type { get; set; }
    public double Amount { get; set; }
    public string Currency { get; set; }
    public DateTime CreatedTime { get; set; }

    [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; }
}
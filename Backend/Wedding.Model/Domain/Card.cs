using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding.Model.Domain;

public class Card : StateEntity<string, string, string, string, int>
{
    [Key] public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    [ForeignKey("CustomerId")] public virtual Customer? Customer { get; set; }
    public string? Code { get; set; }
    public int? TotalCard { get; set; } = 0;
    public float? TotalRate { get; set; } = 0;
    public int? Version { get; set; } = 1;
    public double? TotalEarned { get; set; } = 0;
}
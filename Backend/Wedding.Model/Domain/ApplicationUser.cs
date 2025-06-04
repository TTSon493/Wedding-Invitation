using Microsoft.AspNetCore.Identity;

namespace Wedding.Model.Domain;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public string? TaxNumber { get; set; }
    public DateTime? UpdateTime { get; set; }
    public DateTime? CreateTime { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public bool SendClearEmail { get; set; }
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
}


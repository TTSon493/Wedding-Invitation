using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Wedding.Model.Domain;

public class Guest
{
    [Key]
    public Guid GuestId { get; set; }
    
    public Guid? GuestListId { get; set; }
    [ForeignKey("GuestListId")]
    public virtual GuestList GuestList { get; set; }
    public Guid EventId { get; set; }
    
    [ForeignKey("EventId")]
    public virtual Event Event { get; set; }
    
    public string Name { get; set; }
    public string Attend { get; set; }
    public string Gift { get; set; }
}

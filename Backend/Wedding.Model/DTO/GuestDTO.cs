using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wedding.Model.DTO;

public class GuestDTO
{
    public Guid GuestId { get; set; }
    public Guid EventId { get; set; }
    public string Name { get; set; }
    public string Attend { get; set; }
    public string Gift { get; set; }
}

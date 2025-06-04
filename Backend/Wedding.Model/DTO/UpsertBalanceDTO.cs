using System.Text.Json.Serialization;

namespace Wedding.Model.DTO;

public class UpsertBalanceDTO
{
    public string UserId { get; set; }
    public double AvailableBalance { get; set; }
    public double PayoutBalance { get; set; }
    public string Currency { get; set; }
}
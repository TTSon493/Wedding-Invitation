using System.Text.Json.Serialization;
using Wedding.Utility.Constants;

namespace Wedding.Model.DTO;

public class CreateTransactionDTO
{
    public string UserId { get; set; }
    public StaticEnum.TransactionType Type { get; set; }
    public double Amount { get; set; }
}
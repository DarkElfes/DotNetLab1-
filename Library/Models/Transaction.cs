
namespace Library.Models;


public enum TransactionType
{
    Transfer = 0,
    Deposit,
    Withdraw,
}
public class Transaction
{
    public int Id { get; set; }
    public TransactionType Type { get; set; }
    public int CardId { get; set; }
    public int ToCardId { get; set; }
    public string? Reason { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}

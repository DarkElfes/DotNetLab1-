namespace Library.Models.DTOs;

public class TransactionDTO
{
    public int Id { get; set; }
    public TransactionType Type { get; set; }
    public int CardId { get; set; }
    public string CardUserName { get; set; } = string.Empty;
    public string ToCardUserName { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}

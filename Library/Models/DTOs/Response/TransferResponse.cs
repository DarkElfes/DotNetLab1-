
namespace Library.Models.DTOs.Response;

public class TransferResponse
{
    public decimal CurrentBalance { get; set; }
    public TransactionDTO TransactionDTO { get; set; }
}


namespace Library.Models.DTOs;

public class CardDTO
{
    public int Id { get; set; }
    public string Number { get; set; }
    public decimal Balance { get; set; }
    public List<TransactionDTO> TransactionDTOs { get; set; } = new();
}

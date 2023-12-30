using Library.Models;
namespace Bank.Repository.IRepository;

public interface ICardRepository : IRepository<Card>
{
    Card? GetByNumber(string number);
}

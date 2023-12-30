using Library.Models;

namespace Bank.Repository.IRepository;

public interface IUserRepository : IRepository<User>
{
    User? GetByEmail(string email);
    User? GetByCardId(int cardId);
}

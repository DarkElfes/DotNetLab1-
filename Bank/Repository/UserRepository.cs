using Bank.Data;
using Bank.Repository.IRepository;
using Library.Models;

namespace Bank.Repository;

public class UserRepository(ApplicationDbContext db) : IUserRepository
{
    private readonly ApplicationDbContext _db = db;

    public User Create(User user)
    {
        _db.Users.Add(user);
        Save();

        return user;
    }

    public void Update(User user)
    {
        _db.Users.Update(user);
        Save();
    }

    public void Delete(int id)
    {
        if (GetById(id) is User user)
            _db.Users.Remove(user);
    }

    public IEnumerable<User> GetAll()
        => _db.Users;

    public User? GetById(int id)
        => _db.Users.FirstOrDefault(u => u.Id == id);

    public User? GetByEmail(string email)
        => _db.Users.FirstOrDefault(u => u.Email == email);
    public User? GetByCardId(int cardId)
        => _db.Users.FirstOrDefault(u => u.CardId == cardId);


    public void Save()
        => _db.SaveChanges();

}

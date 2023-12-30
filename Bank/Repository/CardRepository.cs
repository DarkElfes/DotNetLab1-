using Bank.Data;
using Bank.Repository.IRepository;
using Library.Models;

namespace Bank.Repository;

public class CardRepository(ApplicationDbContext db) : ICardRepository
{
    private readonly ApplicationDbContext _db = db;    

    public Card Create(Card card)
    {
        _db.Cards.Add(card);
        Save();

        return card;
    }

    public void Update(Card card)
    {
        _db.Cards.Update(card);
        Save();
    }

    public void Delete(int id)
    {
        if (GetById(id) is Card card)
        {
            _db.Cards.Remove(card);
            Save();
        }
    }

    public IEnumerable<Card> GetAll() 
        => _db.Cards;

    public Card? GetById(int id)
        => _db.Cards.FirstOrDefault(c => c.Id == id);

    public Card? GetByNumber(string number)
        => _db.Cards.FirstOrDefault(c => c.Number == number);

    public void Save()
        => _db.SaveChanges();

}
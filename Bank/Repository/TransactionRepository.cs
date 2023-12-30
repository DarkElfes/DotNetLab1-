using Bank.Data;
using Bank.Repository.IRepository;
using Library.Models;

namespace Bank.Repository;

public class TransactionRepository(ApplicationDbContext db) : IRepository<Transaction>
{
    private readonly ApplicationDbContext _db = db;

    public Transaction Create(Transaction transaction)
    {
        _db.Transactions.Add(transaction);
        Save();
        return transaction;
    }

    public void Update(Transaction transaction)
    {
        _db.Transactions.Update(transaction);
        Save();
    }

    public void Delete(int id)
    {
        if(GetById(id) is Transaction transaction)
            _db.Transactions.Remove(transaction);
    }

    public IEnumerable<Transaction> GetAll()
        => _db.Transactions;

    public Transaction? GetById(int id)
        => _db.Transactions.FirstOrDefault(t => t.Id == id);


    public void Save()
        => _db.SaveChanges();
       
    

}

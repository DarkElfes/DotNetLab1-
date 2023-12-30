namespace Bank.Repository.IRepository;

public interface IRepository<T>
    where T : class
{
    IEnumerable<T> GetAll();
    T? GetById(int id);
    T Create(T entity);
    void Update(T entity);
    void Delete(int id);

    void Save();
}

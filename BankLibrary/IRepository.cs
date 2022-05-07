namespace BankLibrary;

public interface IRepository<T>
{
    bool SaveChangesAsync();

    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task CreateAsync(T account);
}
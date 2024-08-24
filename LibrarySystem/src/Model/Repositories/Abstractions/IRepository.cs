namespace LibrarySystem;

public interface IRepository<T> where T : EntityBase
{
    Task<bool> Save(T item);
    Task<bool> Update(T item);
    Task<bool> Delete(Guid id);
    Task<T?> GetById(Guid id);
    Task<IEnumerable<T>> GetAll();
}

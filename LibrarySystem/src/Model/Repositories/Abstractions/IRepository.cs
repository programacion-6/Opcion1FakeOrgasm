namespace LibrarySystem;

public interface IRepository<T> where T : EntityBase
{
    bool Save(T item);
    bool Update(T item);
    bool Delete(T item);
    T? GetById(Guid itemId);
    List<T> GetAll();
}

namespace LibrarySystem;

public interface IPaginable<T>
{
    public Task<IEnumerable<T>> GetByPage(int pageNumber, int pageSize);
}
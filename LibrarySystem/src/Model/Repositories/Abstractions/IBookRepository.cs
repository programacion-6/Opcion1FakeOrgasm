namespace LibrarySystem;

public interface IBookRepository : IRepository<Book>, IPaginable<Book>
{
    Task<Book?> GetByTitle(string title);
    Task<Book?> GetByAuthor(string author);
    Task<Book?> GetByISBN(string ISBN);
    Task<IEnumerable<Book>> GetBooksByGenre(string genre);
}
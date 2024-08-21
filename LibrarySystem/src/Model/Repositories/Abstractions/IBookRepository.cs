namespace LibrarySystem;

public interface IBookRepository : IRepository<Book>
{
    Book? GetByTitle(string title);
    Book? GetByAuthor(string author);
    Book? GetByISBN(string ISBN);
    List<Book> GetBooksByGenre(string genre);
}
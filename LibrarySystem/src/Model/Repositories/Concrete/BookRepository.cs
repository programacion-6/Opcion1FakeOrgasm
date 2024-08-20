namespace LibrarySystem;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public Book? GetByTitle(string title)
    {
        return Data.Values
                   .FirstOrDefault(book =>
                   book.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }

    public Book? GetByAuthor(string author)
    {
        return Data.Values
                   .FirstOrDefault(book =>
                   book.Author.Equals(author, StringComparison.OrdinalIgnoreCase));
    }

    public Book? GetByISBN(string ISBN)
    {
        return Data.Values
                   .FirstOrDefault(book =>
                   book.ISBN.Equals(ISBN, StringComparison.OrdinalIgnoreCase));
    }

    public List<Book> GetBooksByGenre(string genre)
    {
        return Data.Values
                .Where(book => book.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                .ToList();
    }
}

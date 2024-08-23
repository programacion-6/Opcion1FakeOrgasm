
using System.Data;
using Dapper;

namespace LibrarySystem;

public class BookRepository : IBookRepository
{
    private readonly IDbConnection _connection;

    public BookRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<bool> Save(Book item)
    {
        const string sql = @"
                INSERT INTO Books (Id, Title, Author, ISBN, Genre, PublicationYear)
                VALUES (@Id, @Title, @Author, @ISBN, @Genre, @PublicationYear)";

        int affected = await _connection.ExecuteAsync(sql, item);
        return affected > 0;
    }

    public async Task<bool> Update(Book item)
    {
        const string sql = @"
                UPDATE Books 
                SET Title = @Title, Author = @Author, ISBN = @ISBN, 
                    Genre = @Genre, PublicationYear = @PublicationYear
                WHERE Id = @Id";

        int affected = await _connection.ExecuteAsync(sql, item);
        return affected > 0;
    }

    public async Task<bool> Delete(Guid id)
    {
        const string sql = "DELETE FROM Books WHERE Id = @Id";
        int affected = await _connection.ExecuteAsync(sql, new { Id = id });
        return affected > 0;
    }

    public async Task<Book> GetById(Guid id)
    {
        const string sql = "SELECT * FROM Books WHERE Id = @Id";
        return await _connection.QuerySingleOrDefaultAsync<Book>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Book>> GetAll()
    {
        const string sql = "SELECT * FROM Books";
        return await _connection.QueryAsync<Book>(sql);
    }

    async Task<Book> IBookRepository.GetByTitle(string title)
    {
        const string sql = "SELECT * FROM Books WHERE LOWER(Title) = LOWER(@Title)";
        return await _connection.QuerySingleOrDefaultAsync<Book>(sql, new { Title = title });
    }

    async Task<Book> IBookRepository.GetByAuthor(string author)
    {
        const string sql = "SELECT * FROM Books WHERE LOWER(Author) = LOWER(@Author)";
        return await _connection.QuerySingleOrDefaultAsync<Book>(sql, new { Author = author });
    }

    async Task<Book> IBookRepository.GetByISBN(string ISBN)
    {
        const string sql = "SELECT * FROM Books WHERE LOWER(ISBN) = LOWER(@ISBN)";
        return await _connection.QuerySingleOrDefaultAsync<Book>(sql, new { ISBN });
    }

    async Task<IEnumerable<Book>> IBookRepository.GetBooksByGenre(string genre)
    {
        const string sql = "SELECT * FROM Books WHERE LOWER(Genre) = LOWER(@Genre)";
        return await _connection.QueryAsync<Book>(sql, new { Genre = genre });
    }
}

using Dapper;
using Npgsql;

namespace LibrarySystem;

public class BookRepository : IBookRepository
{
    private readonly string _connectionString;

    public BookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> Save(Book item)
    {
        const string sql = @"
                INSERT INTO Books (Id, Title, Author, ISBN, Genre, PublicationYear)
                VALUES (@Id, @Title, @Author, @ISBN, @Genre, @PublicationYear)";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, item);
            return affected > 0;
        }
    }

    public async Task<bool> Update(Book item)
    {
        const string sql = @"
                UPDATE Books 
                SET Title = @Title, Author = @Author, ISBN = @ISBN, 
                    Genre = @Genre, PublicationYear = @PublicationYear
                WHERE Id = @Id";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, item);
            return affected > 0;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        const string sql = "DELETE FROM Books WHERE Id = @Id";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, new { Id = id });
            return affected > 0;
        }
    }

    public async Task<Book> GetById(Guid id)
    {
        const string sql = "SELECT * FROM Books WHERE Id = @Id";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Book>(sql, new { Id = id });
        }
    }

    public async Task<IEnumerable<Book>> GetAll()
    {
        const string sql = "SELECT * FROM Books";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Book>(sql);
        }
    }

    public async Task<Book> GetByTitle(string title)
    {
        const string sql = "SELECT * FROM Books WHERE LOWER(Title) = LOWER(@Title)";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Book>(sql, new { Title = title });
        }
    }

    public async Task<Book> GetByAuthor(string author)
    {
        const string sql = "SELECT * FROM Books WHERE LOWER(Author) = LOWER(@Author)";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Book>(sql, new { Author = author });
        }
    }

    public async Task<Book> GetByISBN(string ISBN)
    {
        const string sql = "SELECT * FROM Books WHERE LOWER(ISBN) = LOWER(@ISBN)";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Book>(sql, new { ISBN });
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByGenre(string genre)
    {
        const string sql = "SELECT * FROM Books WHERE LOWER(Genre) = LOWER(@Genre)";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Book>(sql, new { Genre = genre });
        }
    }
}

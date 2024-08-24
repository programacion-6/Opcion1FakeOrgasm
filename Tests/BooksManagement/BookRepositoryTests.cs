using LibrarySystem;
using Npgsql;
using Dapper;

namespace LibrarySystem;

public class BookRepositoryTests
{
    private IBookRepository _repository;
    private readonly NpgsqlConnection _connection;

    public BookRepositoryTests()
    {
        string connectionString = "Host=localhost;Port=5432;Database=FakeOrgasm;Username=fakeLibrary;Password=123456789";
        _connection = new NpgsqlConnection(connectionString);
        _repository = new BookRepository(connectionString);
    }

    private async Task InitializeDatabase()
    {
        await _connection.OpenAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.CloseAsync();
    }

    [Fact]
    public async Task Save_ShouldAddNewBook()
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "New Book",
            Author = "New Author",
            ISBN = "1234567890123",
            Genre = "New Genre",
            PublicationYear = 2023
        };

        var result = await _repository.Save(book);
        Assert.True(result);

        var savedBook = await _repository.GetById(book.Id);
        Assert.NotNull(savedBook);
        Assert.Equal(book.Title, savedBook.Title);
    }

    [Fact]
    public async Task Update_ShouldModifyExistingBook()
    {
        var book = new Book
        {
            Id = Guid.Parse("1b9d6bcd-bbfd-4d2a-8cfd-1d62b3d1c1b1"), 
            Title = "Updated Title",
            Author = "Updated Author",
            ISBN = "9780743273565",
            Genre = "Updated Genre",
            PublicationYear = 2023
        };

        var result = await _repository.Update(book);
        Assert.True(result);

        var updatedBook = await _repository.GetById(book.Id);
        Assert.NotNull(updatedBook);
        Assert.Equal(book.Title, updatedBook.Title);
    }

    [Fact]
    public async Task GetById_ShouldReturnCorrectBook()
    {
        var bookId = Guid.Parse("8f7d3e9b-2b6f-4828-a596-7d54210c5fbd");

        var book = await _repository.GetById(bookId);
        Assert.NotNull(book);
        Assert.Equal("War and Peace", book.Title);
    }


    [Fact]
    public async Task GetByTitle_ShouldReturnCorrectBook()
    {
        var title = "1984";

        var book = await _repository.GetByTitle(title);
        Assert.Equal("1984", book.Title);
        Assert.Equal("George Orwell", book.Author);
        Assert.Equal("9780451524935", book.ISBN);
        Assert.Equal("Dystopian", book.Genre);
        Assert.Equal(1949, book.PublicationYear);
    }

    [Fact]
    public async Task GetByAuthor_ShouldReturnCorrectBook()
    {
        var author = "George Orwell";

        var book = await _repository.GetByAuthor(author);
        Assert.NotNull(book);
        Assert.Equal("George Orwell", book.Author);
        Assert.Equal("1984", book.Title);
    }

    [Fact]
    public async Task GetByISBN_ShouldReturnCorrectBook()
    {
        var isbn = "9780451524935"; 

        var book = await _repository.GetByISBN(isbn);
        Assert.NotNull(book);
        Assert.Equal("1984", book.Title);
    }
}
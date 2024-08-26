using LibrarySystem;
using Npgsql;

namespace Tests.Reporting;

public class ReportingTesting
{
    private readonly Reporter _reporter;
    private readonly NpgsqlConnection _connection;

    public ReportingTesting()
    {
        string connectionString =
            "Host=localhost;Port=5432;Database=FakeOrgasm;Username=fakeLibrary;Password=123456789";
        _connection = new NpgsqlConnection(connectionString);

        var patronRepository = new PatronRepository(connectionString);
        _reporter = new Reporter(new LoanRepository(connectionString), new BookRepository(connectionString),
            patronRepository);
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
    public void VerifyAmountBorrowedBooks()
    {
        List<Book> borrowedBooksList = _reporter.GetCurrentlyBorrowedBooks().GetAwaiter().GetResult();
        Assert.Equal(5, borrowedBooksList.Count);
    }
}
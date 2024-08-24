using LibrarySystem;
using Npgsql;

namespace Tests.Search;

public class SearchTesting
{
    private PatronRepository _patronRepository;
    private readonly NpgsqlConnection _connection;

    public SearchTesting()
    {
        string connectionString = "Host=localhost;Port=5432;Database=FakeOrgasm;Username=fakeLibrary;Password=123456789";
        _connection = new NpgsqlConnection(connectionString);
        
        _patronRepository = new PatronRepository(connectionString);
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
    public void VerifyPatronExists()
    {
        Patron alice = _patronRepository.GetByName("Alice Smith").GetAwaiter().GetResult();
        Assert.NotNull(alice);
        Assert.Equal("Alice Smith", alice.Name);
        Assert.Equal(1001, alice.MembershipNumber);
        Assert.Equal("alice.smith@example.com", alice.ContactDetails);
    }

    [Fact]
    public void VerifyPatronNotExists()
    {
        Patron unknown = _patronRepository.GetByName("Reborn").GetAwaiter().GetResult();
        Assert.Null(unknown);
    }
    
    [Fact]
    public void VerifyPatronIsDeleted()
    {
        Patron alice = _patronRepository.GetByName("Alice Smith").GetAwaiter().GetResult();
        Assert.NotNull(alice);
        _patronRepository.Delete(alice.Id);
        alice = _patronRepository.GetByName("Alice Smith").GetAwaiter().GetResult();
        Assert.Null(alice);
    }
    
    [Fact]
    public void VerifyPatronIsUpdated()
    {
        Patron bob = _patronRepository.GetByName("Bob Johnson").GetAwaiter().GetResult();
        Assert.NotNull(bob);
        Assert.Equal("Bob Johnson", bob.Name);
        
        bob.Name = "Bob Gorillaz";
        _patronRepository.Update(bob);
        
        bob = _patronRepository.GetByName("Bob Johnson").GetAwaiter().GetResult();
        Assert.NotNull(bob);
        Assert.Equal("Bob Gorillaz", bob.Name);
    }
    
    [Fact]
    public void VerifyPatronMembershipNumberExists()
    {
        Patron alice = _patronRepository.GetByMembershipNumber(1001).GetAwaiter().GetResult();
        Assert.NotNull(alice);
        Assert.Equal("Alice Smith", alice.Name);
    }
}
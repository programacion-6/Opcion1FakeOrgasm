using Dapper;
using Npgsql;

namespace LibrarySystem;

public class PatronRepository : IPatronRepository
{
    private readonly NpgsqlConnection _connection;

    public PatronRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<bool> Delete(Guid id)
    {
        const string sql = "DELETE FROM Patrons WHERE Id = @Id";
        return await _connection.ExecuteAsync(sql, new { Id = id }) > 0;
    }

    public async Task<IEnumerable<Patron>> GetAll()
    {
        const string sql = "SELECT * FROM Patrons";
        return await _connection.QueryAsync<Patron>(sql);
    }

    public async Task<Patron> GetById(Guid id)
    {
        const string sql = "SELECT * FROM Patrons WHERE Id = @Id";
        return await _connection.QuerySingleOrDefaultAsync<Patron>(sql, new { Id = id });
    }

    public async Task<Patron> GetByMembershipNumber(int membershipNumber)
    {
        const string sql = "SELECT * FROM Patrons WHERE MembershipNumber = @MembershipNumber";
        return await _connection.QuerySingleOrDefaultAsync<Patron>(sql, new { MembershipNumber = membershipNumber });
    }

    public async Task<Patron> GetByName(string name)
    {
        const string sql = "SELECT * FROM Patrons WHERE LOWER(Name) = LOWER(@Name)";
        return await _connection.QuerySingleOrDefaultAsync<Patron>(sql, new { Name = name });
    }

    public async Task<bool> Save(Patron item)
    {
        const string sql = @"
                INSERT INTO Patrons (Id, Name, MembershipNumber, ContactDetails)
                VALUES (@Id, @Name, @MembershipNumber, @ContactDetails)";
               
        int affected = await _connection.ExecuteAsync(sql, item);
        return affected > 0; 
    }

    public async Task<bool> Update(Patron item)
    {
        const string sql = @"
                UPDATE Patrons 
                SET Name = @Name, MembershipNumber = @MembershipNumber, ContactDetails = @ContactDetails
                WHERE Id = @Id";

        int affected = await _connection.ExecuteAsync(sql, item);
        return affected > 0;
    }
}

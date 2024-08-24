using Dapper;
using Npgsql;

namespace LibrarySystem;

public class PatronRepository : IPatronRepository
{
    private readonly string _connectionString;

    public PatronRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> Delete(Guid id)
    {
        const string sql = "DELETE FROM Patrons WHERE Id = @Id";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.ExecuteAsync(sql, new { Id = id }) > 0;
        }
    }

    public async Task<IEnumerable<Patron>> GetAll()
    {
        const string sql = "SELECT * FROM Patrons";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Patron>(sql);
        }
    }

    public async Task<Patron?> GetById(Guid id)
    {
        const string sql = "SELECT * FROM Patrons WHERE Id = @Id";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Patron>(sql, new { Id = id });
        }
    }

    public async Task<Patron?> GetByMembershipNumber(int membershipNumber)
    {
        const string sql = "SELECT * FROM Patrons WHERE MembershipNumber = @MembershipNumber";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Patron>(sql, new { MembershipNumber = membershipNumber });
        }
    }

    public async Task<Patron?> GetByName(string name)
    {
        const string sql = "SELECT * FROM Patrons WHERE LOWER(Name) = LOWER(@Name)";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Patron>(sql, new { Name = name });
        }
    }

    public async Task<bool> Save(Patron item)
    {
        const string sql = @"
                INSERT INTO Patrons (Id, Name, MembershipNumber, ContactDetails)
                VALUES (@Id, @Name, @MembershipNumber, @ContactDetails)";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, item);
            return affected > 0;
        }
    }

    public async Task<bool> Update(Patron item)
    {
        const string sql = @"
                UPDATE Patrons 
                SET Name = @Name, MembershipNumber = @MembershipNumber, ContactDetails = @ContactDetails
                WHERE Id = @Id";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, item);
            return affected > 0;
        }
    }
}

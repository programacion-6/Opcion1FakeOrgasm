using Dapper;
using Npgsql;

namespace LibrarySystem;

public class FineRepository : IFineRepository
{
    private readonly string _connectionString;

    public FineRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> Delete(Guid id)
    {
        const string sql = "DELETE FROM Fines WHERE Id = @Id";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, new { Id = id });
            return affected > 0;
        }
    }

    public async Task<IEnumerable<Fine>> GetActiveFines()
    {
        const string sql = "SELECT * FROM Fines WHERE WasPayed = false";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Fine>(sql);
        }
    }

    public async Task<IEnumerable<Fine>> GetAll()
    {
        const string sql = "SELECT * FROM Fines";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Fine>(sql);
        }
    }

    public async Task<Fine> GetById(Guid id)
    {
        const string sql = "SELECT * FROM Fines WHERE Id = @Id";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Fine>(sql, new { Id = id });
        }
    }

    public async Task<Fine> GetByLoan(Guid loanId)
    {
        const string sql = "SELECT * FROM Fines WHERE LoanId = @LoanId";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Fine>(sql, new { LoanId = loanId });
        }
    }

    public async Task<IEnumerable<Fine>> GetFinesByPatron(Guid patronId)
    {
        const string sql = @"
                SELECT F.*
                FROM Fines F
                JOIN Loans L ON F.LoanId = L.Id
                WHERE L.PatronId = @PatronId";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Fine>(sql, new { PatronId = patronId });
        }
    }

    public async Task<bool> Save(Fine item)
    {
        const string sql = @"
                INSERT INTO Fines (Id, LoanId, FineAmount, WasPayed)
                VALUES (@Id, @LoanId, @FineAmount, @WasPayed)";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, item);
            return affected > 0;
        }
    }

    public async Task<bool> Update(Fine item)
    {
        const string sql = @"
                UPDATE Fines 
                SET LoanId = @LoanId, FineAmount = @FineAmount, WasPayed = @WasPayed
                WHERE Id = @Id";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, item);
            return affected > 0;
        }
    }
}

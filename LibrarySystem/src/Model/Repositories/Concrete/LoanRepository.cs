using Dapper;
using Npgsql;

namespace LibrarySystem;

public class LoanRepository : ILoanRepository
{
    private readonly NpgsqlConnection _connection;

    public LoanRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<bool> Delete(Guid id)
    {
        const string sql = "DELETE FROM Loans WHERE Id = @Id";
        int affected = await _connection.ExecuteAsync(sql, new { Id = id });
        return affected > 0;
    }

    public async Task<IEnumerable<Loan>> GetActiveLoansByPatron(Guid patronId)
    {
        const string sql = "SELECT * FROM Loans WHERE PatronId = @PatronId AND WasReturn = false";
        return await _connection.QueryAsync<Loan>(sql, new { PatronId = patronId });
    }

    public async Task<IEnumerable<Loan>> GetAll()
    {
        const string sql = "SELECT * FROM Loans";
        return await _connection.QueryAsync<Loan>(sql);
    }

    public async Task<Loan> GetById(Guid id)
    {
        const string sql = "SELECT * FROM Loans WHERE Id = @Id";
        return await _connection.QuerySingleOrDefaultAsync<Loan>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Loan>> GetCurrentlyLoans()
    {
        const string sql = "SELECT * FROM Loans WHERE WasReturn = false";
        return await _connection.QueryAsync<Loan>(sql);
    }

    public async Task<IEnumerable<Loan>> GetLoansByPatron(Guid patronId)
    {
        const string sql = "SELECT * FROM Loans WHERE PatronId = @PatronId";
        return await _connection.QueryAsync<Loan>(sql, new { PatronId = patronId });
    }

    public async Task<IEnumerable<Loan>> GetOverdueLoans()
    {
        const string sql = "SELECT * FROM Loans WHERE ReturnDate < @CurrentDate AND WasReturn = false";
        return await _connection.QueryAsync<Loan>(sql, new { CurrentDate = DateTime.Now });
    }

    public async Task<bool> Save(Loan item)
    {
        const string sql = @"
                INSERT INTO Loans (Id, BookId, PatronId, LoanDate, ReturnDate, WasReturn)
                VALUES (@Id, @BookId, @PatronId, @LoanDate, @ReturnDate, @WasReturn)";
        
        int affected = await _connection.ExecuteAsync(sql, item);
        return affected > 0;
    }

    public async Task<bool> Update(Loan item)
    {
        const string sql = @"
                UPDATE Loans 
                SET BookId = @BookId, PatronId = @PatronId, LoanDate = @LoanDate, 
                    ReturnDate = @ReturnDate, WasReturn = @WasReturn
                WHERE Id = @Id";
        
        int affected = await _connection.ExecuteAsync(sql, item);
        return affected > 0;
    }
}

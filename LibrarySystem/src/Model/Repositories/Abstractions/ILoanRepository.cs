namespace LibrarySystem;

public interface ILoanRepository : IRepository<Loan>
{
    Task<IEnumerable<Loan>> GetCurrentlyLoans();
    Task<IEnumerable<Loan>> GetOverdueLoans();
    Task<IEnumerable<Loan>> GetLoansByPatron(Guid patronId);
    Task<IEnumerable<Loan>> GetActiveLoansByPatron(Guid patronId);
}
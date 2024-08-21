namespace LibrarySystem;

public interface ILoanRepository : IRepository<Loan>
{
    List<Loan> GetCurrentlyLoans();
    List<Loan> GetOverdueLoans();
    List<Loan> GetLoansByPatron(Patron patron);
    List<Loan> GetActiveLoansByPatron(Patron patron);
}
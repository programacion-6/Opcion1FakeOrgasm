namespace LibrarySystem;

public class LoanRepository : BaseRepository<Loan>, ILoanRepository
{
    public List<Loan> GetCurrentlyLoans()
    {
        return Data.Values
                   .Where(loan => !loan.WasReturn)
                   .ToList();
    }

    public List<Loan> GetOverdueLoans()
    {
        return Data.Values
                   .Where(loan => loan.ReturnDate < DateTime.Now)
                   .ToList();
    }

    public List<Loan> GetLoansByPatron(Patron patron)
    {
        return Data.Values
                   .Where(loan => loan.IdPatron == patron.Id)
                   .ToList();
    }

    public List<Loan> GetActiveLoansByPatron(Patron patron)
    {
        return Data.Values
                   .Where(loan => loan.IdPatron == patron.Id)
                   .Where(loan => !loan.WasReturn)
                   .ToList();
    }
}

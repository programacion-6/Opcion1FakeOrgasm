namespace LibrarySystem;

public class Reporter
{
    private readonly ILoanRepository loanRepository;

    public Reporter(ILoanRepository loanRepository)
    {
        this.loanRepository = loanRepository;
    }

    public List<Book> GetCurrentlyBorrowedBooks()
    {
        var currentlyLoans = loanRepository.GetCurrentlyLoans();
        var currentlyBorrowedBooks = currentlyLoans.Select(loan => loan.Book).ToList();
        return currentlyBorrowedBooks;
    }

    public List<Patron> GetPatternsThatBorrowedBooks()
    {
        var currentlyLoans = loanRepository.GetCurrentlyLoans();
        var patrons = currentlyLoans.Select(loan => loan.Patron).ToList();
        return patrons;
    }

    public List<Book> GetOverdueBooks()
    {
        var overdueLoans = loanRepository.GetOverdueLoans();
        var overdueBooks = overdueLoans.Select(loan => loan.Book).ToList();
        return overdueBooks;
    }

    public List<Loan> GetLoansByPatron(Patron patron)
    {
        return loanRepository.GetLoansByPatron(patron);
    }
}
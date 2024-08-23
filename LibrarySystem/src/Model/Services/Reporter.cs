namespace LibrarySystem;

public class Reporter
{
    private readonly ILoanRepository _loanRepository;

    private readonly IBookRepository _bookRepository;

    private readonly IPatronRepository _patronRepository;

    public Reporter(ILoanRepository loanRepository, IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public List<Book> GetCurrentlyBorrowedBooks()
    {
        var currentlyLoans = _loanRepository.GetCurrentlyLoans();

        var borrowedBookIds = currentlyLoans.Select(loan => loan.IdBook).ToList();

        var currentlyBorrowedBooks = borrowedBookIds
            .Select(id => _bookRepository.GetById(id))
            .Where(book => book != null)
            .ToList();

        return currentlyBorrowedBooks!;
    }

    public List<Patron> GetPatternsThatBorrowedBooks()
    {
        var currentlyLoans = _loanRepository.GetCurrentlyLoans();

        var patronIds = currentlyLoans
            .Select(loan => loan.IdPatron)
            .Distinct()
            .ToList();

        var patrons = patronIds
            .Select(id => _patronRepository.GetById(id))
            .Where(patron => patron != null)
            .ToList();

        return patrons;
    }


    public List<Book> GetOverdueBooks()
    {
        var overdueLoans = _loanRepository.GetOverdueLoans();

        var overdueBookIds = overdueLoans
            .Select(loan => loan.IdBook)
            .Distinct()
            .ToList();

        var overdueBooks = overdueBookIds
            .Select(id => _bookRepository.GetById(id))
            .Where(book => book != null)
            .ToList();

        return overdueBooks;
    }

    public List<Loan> GetLoansByPatron(Patron patron)
    {
        return _loanRepository.GetLoansByPatron(patron);
    }
}
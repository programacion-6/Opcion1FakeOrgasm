namespace LibrarySystem;

public class Reporter
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;

    public Reporter(ILoanRepository loanRepository, IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _patronRepository = patronRepository;
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
    }

    public async Task<List<Book>> GetCurrentlyBorrowedBooks()
    {
        var currentlyLoans = await _loanRepository.GetCurrentlyLoans();
        var currentlyBorrowedBooks = new List<Book>();

        foreach (var loan in currentlyLoans)
        {
            var book = await _bookRepository.GetById(loan.BookId);
            if (book is not null)
            {
                currentlyBorrowedBooks.Add(book);
            }
        }

        return currentlyBorrowedBooks;
    }

    public async Task<List<Patron>> GetPatternsThatBorrowedBooks()
    {
        var currentlyLoans = await _loanRepository.GetCurrentlyLoans();
        var currentlyBorrowedPatrons = new List<Patron>();

        foreach (var loan in currentlyLoans)
        {
            var patron = await _patronRepository.GetById(loan.PatronId);
            if (patron is not null)
            {
                currentlyBorrowedPatrons.Add(patron);
            }
        }

        return currentlyBorrowedPatrons;
    }

    public async Task<List<Book>> GetOverdueBooks()
    {
        var overdueLoans = await _loanRepository.GetOverdueLoans();
        var overdueBooks = new List<Book>();

        foreach (var loan in overdueLoans)
        {
            var book = await _bookRepository.GetById(loan.BookId);
            if (book is not null)
            {
                overdueBooks.Add(book);
            }
        }

        return overdueBooks;
    }

    public async Task<List<Loan>> GetLoansByPatron(Patron patron)
    {
        return (await _loanRepository.GetLoansByPatron(patron.Id)).ToList();
    }
}
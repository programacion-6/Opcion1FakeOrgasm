namespace LibrarySystem.Reports;

public class CurrentlyBorrowedBooksReporter
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;

    public CurrentlyBorrowedBooksReporter(ILoanRepository loanRepository, IBookRepository bookRepository)
    {
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
}
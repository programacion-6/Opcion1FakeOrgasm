namespace LibrarySystem.Reports;

public class OverdueBooksReporter
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;

    public OverdueBooksReporter(ILoanRepository loanRepository, IBookRepository bookRepository)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
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
}
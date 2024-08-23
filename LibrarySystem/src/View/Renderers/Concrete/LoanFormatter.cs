using LibrarySystem;

public class LoanFormatter
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;

    public LoanFormatter(ILoanRepository loanRepository, IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public string FormatLoan(Guid loanId)
    {
        var loan = _loanRepository.GetById(loanId);
        if (loan == null) return "Loan not found";

        var book = _bookRepository.GetById(loan.IdBook);
        var patron = _patronRepository.GetById(loan.IdPatron);

        string bookTitle = book != null ? book.Title : "Unknown Book";
        string patronName = patron != null ? patron.Name : "Unknown Patron";

        return $"Loan {(loan.WasReturn ? "returned" : "active")} | {loan.LoanDate} - {loan.ReturnDate}"+ 
                $"\n\tBook: {bookTitle}" +
                $"\n\tPatron: {patronName}";
    }
}
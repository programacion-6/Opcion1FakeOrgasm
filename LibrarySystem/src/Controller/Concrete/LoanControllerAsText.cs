using Spectre.Console;

namespace LibrarySystem;

public class LoanControllerAsText : IExecutableHandler<string>
{
    private Lender _lender;
    private ILoanRepository _loanRepository;
    private IPatronRepository _patronRepository;
    private IBookRepository _bookRepository;
    private IMessageRenderer _messageRenderer;
    private EntitySelectorByConsole<Patron> _patronSelector;
    private EntitySelectorByConsole<Book> _bookSelector;

    private ErrorLogger _errorLogger;

    public LoanControllerAsText(Lender lender, ILoanRepository loanRepository, IPatronRepository patronRepository, IBookRepository bookRepository, IMessageRenderer messageRenderer, EntitySelectorByConsole<Patron> patronSelector, EntitySelectorByConsole<Book> bookSelector)
    {
        _lender = lender;
        _loanRepository = loanRepository;
        _patronRepository = patronRepository;
        _bookRepository = bookRepository;
        _messageRenderer = messageRenderer;
        _patronSelector = patronSelector;
        _bookSelector = bookSelector;
        _errorLogger = new ErrorLogger();
    }

    public async Task Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "lend":
                await LendBook();
                break;
            case "return":
                await ReturnBook();
                break;
            default:
                _messageRenderer.RenderErrorMessage("option not found");
                break;
        }
    }

    private bool ValidateBookFound(Book? book)
    {
        var wasFound = true;
        if (book is null)
        {
            _messageRenderer.RenderInfoMessage("there may not be any books available to borrow...");

            wasFound = false;
        }

        return wasFound;
    }

    private bool ValidatePatronFound(Patron? patron)
    {
        var wasFound = true;
        if (patron is null)
        {
            _messageRenderer.RenderInfoMessage("at least one patron must be registered");

            wasFound = false;
        }

        return wasFound;
    }

    private async Task<Patron?> SelectPatronFromLoans()
    {
        var activeLoans = await _loanRepository.GetCurrentlyLoans();

        var patronsIds = activeLoans
            .Select(loan => loan.PatronId)
            .Distinct()
            .ToList();

        var patrons = await Task.WhenAll(patronsIds.Select(async patronId => await _patronRepository.GetById(patronId)));
        var patron = await _patronSelector.TryToSelectAtLeastOne(patrons.OfType<Patron>().ToList());
        return patron;
    }

    private async Task<Book?> SelectBookFromPatronLoans(IEnumerable<Loan> patronLoans)
    {
        var borrowedBooks = await Task.WhenAll(
                                patronLoans.Select(async loan =>
                                await _bookRepository.GetById(loan.BookId)));

        var bookSelected = await _bookSelector.TryToSelectAtLeastOne(borrowedBooks.OfType<Book>().ToList());
        return bookSelected;
    }

    private async Task ReturnBook()
    {
        var patron = await SelectPatronFromLoans();
        if (patron is not null)
        {
            var patronLoans = await _loanRepository.GetActiveLoansByPatron(patron.Id);
            var bookSelected = await SelectBookFromPatronLoans(patronLoans);
            if (bookSelected is not null)
            {
                var loanSelected = patronLoans.FirstOrDefault(loan => loan.BookId == bookSelected.Id);
                if (loanSelected is not null)
                {
                    await _lender.ReturnBook(loanSelected);
                    _messageRenderer.RenderSuccessMessage("returned book");
                }
            }
        }
    }

    private async Task<Book?> SelectNoBorrowedBook()
    {
        var allLoans = await _loanRepository.GetCurrentlyLoans();
        var borrowedBooksIds = allLoans
                                .Select(loan => loan.BookId)
                                .ToList();

        var allBooks = await _bookRepository.GetAll();
        var booksAvailable = allBooks
                                .Where(book => !borrowedBooksIds.Contains(book.Id))
                                .ToList();

        var book = await _bookSelector.TryToSelectAtLeastOne(booksAvailable);
        return book;
    }

    private async Task<Patron?> SelectPatronForLoan()
    {
        var allPatrons = await _patronRepository.GetAll();

        var patron = await _patronSelector.TryToSelectAtLeastOne(allPatrons.ToList());
        return patron;
    }

    private async Task LendBookToPatron(Book book, Patron patron)
    {
        try
        {
            int loanTimeInDays = AnsiConsole.Ask<int>("Enter the [bold]days of loan[/]:");
            await _lender.LendBook(book, patron, loanTimeInDays);
            _messageRenderer.RenderSuccessMessage("successful loan");
        }
        catch (LoanException ex)
        {
            _errorLogger.LogErrorBasedOnSeverity(ex.Severity, ex.Message, ex);
            _messageRenderer.RenderErrorMessage($"{ex.Message} \n...{ex.ResolutionSuggestion}");
        }
        catch (Exception ex)
        {
            _errorLogger.LogErrorBasedOnSeverity(SeverityLevel.High, "", ex);
            _messageRenderer.RenderErrorMessage(ex.Message);

        }
    }

    private async Task LendBook()
    {
        var book = await SelectNoBorrowedBook();
        var patron = await SelectPatronForLoan();
        var isValidToLoan = ValidateBookFound(book) && ValidatePatronFound(patron);

        if (isValidToLoan)
        {
#pragma warning disable CS8604
            // null data validation was already done previously
            // this is by the C# compiler
            await LendBookToPatron(book, patron);
#pragma warning restore CS8604
        }
    }

}
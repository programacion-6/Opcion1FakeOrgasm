namespace LibrarySystem;

public class LoanControllerAsText : IExecutableHandler<string>
{
    private Lender _lender;
    private IReceiver<string> _receiver;
    private ILoanRepository _loanRepository;
    private IPatronRepository _patronRepository;
    private IBookRepository _bookRepository;
    private IMessageRenderer _messageRenderer;
    private EntitySelectorByConsole<Patron> _patronSelector;
    private EntitySelectorByConsole<Book> _bookSelector;

    public LoanControllerAsText(Lender lender, ILoanRepository loanRepository, IPatronRepository patronRepository, IBookRepository bookRepository, IReceiver<string> receiver, IMessageRenderer messageRenderer, EntitySelectorByConsole<Patron> patronSelector, EntitySelectorByConsole<Book> bookSelector)
    {
        _lender = lender;
        _loanRepository = loanRepository;
        _patronRepository = patronRepository;
        _bookRepository = bookRepository;
        _receiver = receiver;
        _messageRenderer = messageRenderer;
        _patronSelector = patronSelector;
        _bookSelector = bookSelector;
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

    private bool TheBookWasFound(Book? book)
    {
        var wasFound = true;
        if (book is null)
        {
            _messageRenderer.RenderInfoMessage("there may not be any books available to borrow...");

            wasFound = false;
        }

        return wasFound;
    }

    private bool ThePatronWasFound(Patron? patron)
    {
        var wasFound = true;
        if (patron is null)
        {
            _messageRenderer.RenderInfoMessage("at least one patron must be registered");

            wasFound = false;
        }

        return wasFound;
    }

    private async Task ReturnBook()
    {
        var patrons = await GetActivePatrons();
        var patron = _patronSelector.TryToSelectAtLeastOne(patrons);

        if (patron is not null)
        {
            await HandleBookReturnForPatron(patron);
        }
    }

    private async Task<List<Patron>> GetActivePatrons()
    {
        var activeLoans = await _loanRepository.GetCurrentlyLoans();
        var patronsIds = activeLoans.Select(loan => loan.PatronId).Distinct().ToList();

        return await Task.WhenAll(patronsIds.Select(async patronId => await _patronRepository.GetById(patronId))).ContinueWith(task => task.Result.ToList());
    }

    private async Task HandleBookReturnForPatron(Patron patron)
    {
        var patronLoans = await GetLoansForPatron(patron.Id);
        var borrowedBooks = await GetBooksFromLoans(patronLoans);

        var bookSelected = _bookSelector.TryToSelectAtLeastOne(borrowedBooks);

        if (bookSelected is not null)
        {
            await ReturnSelectedBook(patronLoans, bookSelected);
        }
    }

    private async Task<List<Loan>> GetLoansForPatron(Guid patronId)
    {
        var loans = await _loanRepository.GetActiveLoansByPatron(patronId);
        return loans.ToList();
    }


    private async Task<List<Book>> GetBooksFromLoans(List<Loan> loans)
    {
        return await Task.WhenAll(loans.Select(async loan => await _bookRepository.GetById(loan.BookId))).ContinueWith(task => task.Result.ToList());
    }

    private async Task ReturnSelectedBook(List<Loan> patronLoans, Book bookSelected)
    {
        var loanSelected = patronLoans.FirstOrDefault(loan => loan.BookId == bookSelected.Id);

        if (loanSelected is not null)
        {
            await _lender.ReturnBook(loanSelected);
            _messageRenderer.RenderSuccessMessage("Returned book");
        }
    }


   private async Task LendBook()
{
    var booksAvailable = await GetAvailableBooks();
    var patron = await SelectPatron();
    
    if (booksAvailable.Any() && patron != null)
    {
        var book = _bookSelector.TryToSelectAtLeastOne(booksAvailable);

        if (IsValidLoan(book, patron))
        {
            await ProcessLoan(book, patron);
        }
    }
}

private async Task<List<Book>> GetAvailableBooks()
{
    var allLoans = await _loanRepository.GetCurrentlyLoans();
    var borrowedBooksIds = allLoans.Select(loan => loan.BookId).ToList();

    var allBooks = await _bookRepository.GetAll();
    return allBooks.Where(book => !borrowedBooksIds.Contains(book.Id)).ToList();
}

private async Task<Patron> SelectPatron()
{
    var allPatrons = await _patronRepository.GetAll();
    return _patronSelector.TryToSelectAtLeastOne(allPatrons.ToList());
}

private bool IsValidLoan(Book book, Patron patron)
{
    return TheBookWasFound(book) && ThePatronWasFound(patron);
}

private async Task ProcessLoan(Book book, Patron patron)
{
    try
    {
        int loanTimeInDays = GetLoanTime();
        await _lender.LendBook(book, patron, loanTimeInDays);
        _messageRenderer.RenderSuccessMessage("Successful loan");
    }
    catch (LoanException ex)
    {
        _messageRenderer.RenderErrorMessage($"{ex.Message} \n...{ex.ResolutionSuggestion}");
    }
    catch (Exception ex)
    {
        _messageRenderer.RenderErrorMessage(ex.Message);
    }
}

private int GetLoanTime()
{
    _messageRenderer.RenderSimpleMessage("Enter the days of loan:");
    int loanTimeInDays;
    string loanTimeInput;
    bool isValidTime;

    do
    {
        loanTimeInput = _receiver.ReceiveInput();
        isValidTime = int.TryParse(loanTimeInput, out loanTimeInDays);
        if (!isValidTime)
        {
            _messageRenderer.RenderErrorMessage("Enter a number please");
        }
    } while (!isValidTime);

    return loanTimeInDays;
}


}
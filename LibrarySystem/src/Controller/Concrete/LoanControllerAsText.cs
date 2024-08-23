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

    public void Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "lend":
                LendBook();
                break;
            case "return":
                ReturnBook();
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

    private void ReturnBook()
    {
        var patron = SelectPatronForReturn();
        if (patron != null)
        {
            var bookSelected = SelectBookForReturn(patron);
            if (bookSelected != null)
            {
                ReturnSelectedBook(patron, bookSelected);
            }
        }
    }

    private Patron? SelectPatronForReturn()
    {
        var activeLoans = _loanRepository.GetCurrentlyLoans();

        var patronIds = activeLoans
            .Select(loan => loan.IdPatron)
            .Distinct()
            .ToList();

        var patrons = patronIds
            .Select(id => _patronRepository.GetById(id))
            .Where(patron => patron != null)
            .ToList();

        return _patronSelector.TryToSelectAtLeastOne(patrons);
    }

    private Book? SelectBookForReturn(Patron patron)
    {
        var patronLoans = _loanRepository.GetActiveLoansByPatron(patron);
        
        var borrowedBooks = patronLoans
            .Select(loan => _bookRepository.GetById(loan.IdBook))
            .Where(book => book != null)
            .ToList();
        
        return _bookSelector.TryToSelectAtLeastOne(borrowedBooks);
    }

    private void ReturnSelectedBook(Patron patron, Book bookSelected)
    {
        var loanSelected = _loanRepository.GetActiveLoansByPatron(patron)
                                          .FirstOrDefault(loan => loan.IdBook == bookSelected.Id);
        if (loanSelected != null)
        {
            _lender.ReturnBook(loanSelected);
            _messageRenderer.RenderSuccessMessage("returned book");
        }
    }

    private void LendBook()
    {
        var book = SelectAvailableBook();
        var patron = SelectPatron();
        var isValidToLoan = ValidateLoanEligibility(book, patron);

        if (isValidToLoan)
        {
            ProcessLoan(book, patron);
        }
    }

    private Book SelectAvailableBook()
    {
        var borrowedBooksIds = _loanRepository.GetCurrentlyLoans()
                                              .Select(loan => loan.IdBook)
                                              .ToList();
        var booksAvailable = _bookRepository.GetAll()
                                            .Where(book => !borrowedBooksIds.Contains(book.Id))
                                            .ToList();

        return _bookSelector.TryToSelectAtLeastOne(booksAvailable);
    }

    private Patron SelectPatron()
    {
        var allPatrons = _patronRepository.GetAll();

        return _patronSelector.TryToSelectAtLeastOne(allPatrons);
    }

    private bool ValidateLoanEligibility(Book book, Patron patron)
    {
        return TheBookWasFound(book) && ThePatronWasFound(patron);
    }

    private void ProcessLoan(Book book, Patron patron)
    {
        try
        {
            int loanTimeInDays = GetLoanTimeInDays();
            _lender.LendBook(book, patron, loanTimeInDays);
            _messageRenderer.RenderSuccessMessage("successful loan");
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

    private int GetLoanTimeInDays()
    {
        _messageRenderer.RenderSimpleMessage("enter the days of loan:");
        int loanTimeInDays;
        string loanTimeInput;
        bool isValidTime;
        do
        {
            loanTimeInput = _receiver.ReceiveInput();
            isValidTime = int.TryParse(loanTimeInput, out loanTimeInDays);
            if (!isValidTime)
            {
                _messageRenderer.RenderErrorMessage("enter a number plase");
            }
        } while (!isValidTime);

        return loanTimeInDays;
    }
}
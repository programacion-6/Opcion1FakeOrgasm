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
        var activeLoans = await _loanRepository.GetCurrentlyLoans();

        var patronsIds = activeLoans
            .Select(loan => loan.PatronId)
            .Distinct()
            .ToList();

        var patrons = await Task.WhenAll(patronsIds.Select(async patronId => await _patronRepository.GetById(patronId)));


        var patron = _patronSelector.TryToSelectAtLeastOne(patrons.ToList());

        if (patron is not null)
        {
            var patronLoans = await _loanRepository.GetActiveLoansByPatron(patron.Id);

            var borrowedBooks = await Task.WhenAll(
            patronLoans.Select(async loan => await _bookRepository.GetById(loan.BookId)));

            var bookSelected = _bookSelector.TryToSelectAtLeastOne(borrowedBooks.ToList());
            if (bookSelected is not null)
            {
                var loanSelected = patronLoans.FirstOrDefault(loan => loan.BookId == bookSelected.Id);
#pragma warning disable CS8604
                await _lender.ReturnBook(loanSelected);
#pragma warning restore CS8604
                _messageRenderer.RenderSuccessMessage("returned book");

            }
        }
    }

    private async Task LendBook()
    {
        var borrowedBooksIds = (await _loanRepository.GetCurrentlyLoans())
                                .Select(loan => loan.BookId)
                                .ToList();

        var booksAvailable = (await _bookRepository.GetAll())
                                .Where(book => !borrowedBooksIds.Contains(book.Id))
                                .ToList();

        var book = _bookSelector.TryToSelectAtLeastOne(booksAvailable);

        var allPatrons = await _patronRepository.GetAll();
        var patron = _patronSelector.TryToSelectAtLeastOne(allPatrons.ToList());
        var isValidToLoan = TheBookWasFound(book) && ThePatronWasFound(patron);

        if (isValidToLoan)
        {
            try
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

#pragma warning disable CS8604
                await _lender.LendBook(book, patron, loanTimeInDays);
#pragma warning restore CS8604
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
    }

}
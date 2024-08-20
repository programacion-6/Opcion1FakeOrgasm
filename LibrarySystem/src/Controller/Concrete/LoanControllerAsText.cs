﻿namespace LibrarySystem;

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
        var activeLoans = _loanRepository.GetCurrentlyLoans();
        var patrons = activeLoans.Select(loan => loan.Patron).ToList();
        var patron = _patronSelector.TryToSelectAtLeastOne(patrons);

        if (patron is not null)
        {
            var patronLoans = _loanRepository.GetActiveLoansByPatron(patron);
            var borrowedBooks = patronLoans.Select(loan => loan.Book).ToList();
            var bookSelected = _bookSelector.TryToSelectAtLeastOne(borrowedBooks);
            if (bookSelected is not null)
            {
                var loanSelected = patronLoans.FirstOrDefault(loan => loan.Book.Id == bookSelected.Id);
#pragma warning disable CS8604
                _lender.ReturnBook(loanSelected);
#pragma warning restore CS8604
                _messageRenderer.RenderSuccessMessage("returned book");

            }
        }
    }

    private void LendBook()
    {
        var borrowedBooks = _loanRepository.GetCurrentlyLoans().Select(loan => loan.Book).ToList();
        var booksAvailable = _bookRepository.GetAll().Where(book => !borrowedBooks.Contains(book)).ToList();
        var book = _bookSelector.TryToSelectAtLeastOne(booksAvailable);

        var allPatrons = _patronRepository.GetAll();
        var patron = _patronSelector.TryToSelectAtLeastOne(allPatrons);
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
                _lender.LendBook(book, patron, loanTimeInDays);
#pragma warning restore CS8604
                _messageRenderer.RenderSuccessMessage("successful loan");

            }
            catch (Exception ex)
            {
                _messageRenderer.RenderErrorMessage(ex.Message);

            }
        }
    }

}
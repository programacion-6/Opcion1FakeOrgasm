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
        var patrons = activeLoans.Select(loan => loan.Patron)
                                 .GroupBy(patron => patron.Id)
                                 .Select(group => group.First())
                                 .ToList();
        return _patronSelector.TryToSelectAtLeastOne(patrons);
    }

    private Book? SelectBookForReturn(Patron patron)
    {
        var patronLoans = _loanRepository.GetActiveLoansByPatron(patron);
        var borrowedBooks = patronLoans.Select(loan => loan.Book).ToList();
        return _bookSelector.TryToSelectAtLeastOne(borrowedBooks);
    }

    private void ReturnSelectedBook(Patron patron, Book bookSelected)
    {
        var loanSelected = _loanRepository.GetActiveLoansByPatron(patron)
                                          .FirstOrDefault(loan => loan.Book.Id == bookSelected.Id);
        if (loanSelected != null)
        {
            _lender.ReturnBook(loanSelected);
            _messageRenderer.RenderSuccessMessage("returned book");
        }
    }

    private void LendBook()
    {
        var book = SelectBookToLend();
        if (book != null)
        {
            var patron = SelectPatronForLending();
            if (patron != null)
            {
                if (IsValidLoan(book, patron, out int loanTimeInDays))
                {
                    TryToLendBook(book, patron, loanTimeInDays);
                }
            }
        }
    }

    private Book? SelectBookToLend()
    {
        var borrowedBooksIds = _loanRepository.GetCurrentlyLoans()
                                              .Select(loan => loan.Book.Id)
                                              .ToList();
        var booksAvailable = _bookRepository.GetAll()
                                            .Where(book => !borrowedBooksIds.Contains(book.Id))
                                            .ToList();
        return _bookSelector.TryToSelectAtLeastOne(booksAvailable);
    }

    private Patron? SelectPatronForLending()
    {
        var allPatrons = _patronRepository.GetAll();
        return _patronSelector.TryToSelectAtLeastOne(allPatrons);
    }

    private bool IsValidLoan(Book book, Patron patron, out int loanTimeInDays)
    {
        _messageRenderer.RenderSimpleMessage("enter the days of loan:");
        string loanTimeInput;
        bool isValidTime;

        do
        {
            loanTimeInput = _receiver.ReceiveInput();
            isValidTime = int.TryParse(loanTimeInput, out loanTimeInDays);
            if (!isValidTime)
            {
                _messageRenderer.RenderErrorMessage("enter a number please");
            }
        } while (!isValidTime);

        return TheBookWasFound(book) && ThePatronWasFound(patron);
    }

    private void TryToLendBook(Book book, Patron patron, int loanTimeInDays)
    {
        try
        {
            _lender.LendBook(book, patron, loanTimeInDays);
            _messageRenderer.RenderSuccessMessage("successful loan");
        }
        catch (Exception ex)
        {
            _messageRenderer.RenderErrorMessage(ex.Message);
        }
    }

}
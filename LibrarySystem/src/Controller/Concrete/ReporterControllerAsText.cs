﻿namespace LibrarySystem;

public class ReporterControllerAsText : IExecutableHandler<string>
{
    private Reporter _reporter;
    private StatisticsGenerator _statisticsGenerator;
    private IPatronRepository _patronRepository;
    private IMessageRenderer _messageRenderer;
    private IEntityFormatterFactory<Book> _bookFormatterFactory;
    private IEntityFormatterFactory<Patron> _patronFormatterFactory;
    private IEntityFormatterFactory<Loan> _loanFormatterFactory;
    private IEntityFormatterFactory<Fine> _fineFormatterFactory;
    private EntitySelectorByConsole<Patron> _patronSelector;


    public ReporterControllerAsText(Reporter reporter, StatisticsGenerator statisticsGenerator, IPatronRepository patronRepository, IEntityFormatterFactory<Book> bookFormatterFactory, IEntityFormatterFactory<Patron> patronFormatterFactory, IEntityFormatterFactory<Loan> loanFormatterFactory, IMessageRenderer messageRenderer, EntitySelectorByConsole<Patron> patronSelector, IEntityFormatterFactory<Fine> fineFormatterFactory)
    {
        _reporter = reporter;
        _statisticsGenerator = statisticsGenerator;
        _patronRepository = patronRepository;

        _bookFormatterFactory = bookFormatterFactory;
        _patronFormatterFactory = patronFormatterFactory;
        _loanFormatterFactory = loanFormatterFactory;
        _messageRenderer = messageRenderer;
        _patronSelector = patronSelector;
        _fineFormatterFactory = fineFormatterFactory;
    }

    public async Task Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "show overdue books":
                await ShowOverdueBooks();
                break;
            case "show borrowed books":
                await ShowCurrentlyBorrowedBooks();
                break;
            case "show current loans by patron":
                await ShowCurrentLoansByPatron();
                break;
            case "show loans by patron":
                await ShowLoansByPatron();
                break;
            case "show most borrowed books":
                await ShowMostBorrowedBooks();
                break;
            case "show most active patrons":
                await ShowMostActivePatrons();
                break;
            case "show patron debts":
                await ShowPatronsFines();
                break;
            default:
                _messageRenderer.RenderErrorMessage("option not found");
                break;
        }
    }

    public async Task ShowOverdueBooks()
    {
        var books = await _reporter.GetOverdueBooks();
        var booksFormatted = books.Select
            (book => _bookFormatterFactory
                    .CreateFormatter(book, FormatType.Simple))
                    .ToList();
        ResultRenderer.RenderResults(booksFormatted);
    }

    public async Task ShowCurrentlyBorrowedBooks()
    {
        var books = await _reporter.GetCurrentlyBorrowedBooks();
        var booksFormatted = books.Select
            (book => _bookFormatterFactory
                    .CreateFormatter(book, FormatType.Simple))
                    .ToList();
        ResultRenderer.RenderResults(booksFormatted);
    }

    public async Task ShowCurrentLoansByPatron()
    {
        var allPatrons = (await _reporter.GetPatternsThatBorrowedBooks())
                                .GroupBy(patron => patron.Id)
                                .Select(group => group.First())
                                .ToList();
        var patron = _patronSelector.TryToSelectAtLeastOne(allPatrons);
        if (patron is not null)
        {
            var loans = await _reporter.GetLoansByPatron(patron);
            var loansFormatted = loans.Select
                    (loan => _loanFormatterFactory
                            .CreateFormatter(loan, FormatType.Simple))
                            .ToList();
            ResultRenderer.RenderResults(loansFormatted);
        }
    }

    public async Task ShowLoansByPatron()
    {
        var allPatrons = await _patronRepository.GetAll();
        var patron = _patronSelector.TryToSelectAtLeastOne(allPatrons.ToList());
        if (patron is not null)
        {
            var loans = await _reporter.GetLoansByPatron(patron);
            if (loans.Any())
            {
                var loansFormatted = loans.Select
                        (loan => _loanFormatterFactory
                            .CreateFormatter(loan, FormatType.Simple))
                            .ToList();
                ResultRenderer.RenderResults(loansFormatted);
            }
            else
            {
                _messageRenderer.RenderInfoMessage("without history...");
            }
        }
    }

    public async Task ShowMostBorrowedBooks()
    {
        var books = await _statisticsGenerator.GetMostBorrowedBooks();
        var booksFormatted = books.Select
            (book => _bookFormatterFactory
                    .CreateFormatter(book, FormatType.Simple))
                    .ToList();
        ResultRenderer.RenderResults(booksFormatted);
    }

    public async Task ShowMostActivePatrons()
    {
        var patrons = await _statisticsGenerator.GetMostActivePatrons();
        var patronsFormatted = patrons.Select
            (patron => _patronFormatterFactory
                    .CreateFormatter(patron, FormatType.Simple))
                    .ToList();
        ResultRenderer.RenderResults(patronsFormatted);
    }

    public async Task ShowPatronsFines()
    {
        var patronsFines = await _statisticsGenerator.GetPatronsFines();
        if (patronsFines.Any())
        {
            foreach (var tuple in patronsFines)
            {
                var patron = tuple.Item1;
                var patronFormatted = _patronFormatterFactory.CreateFormatter(patron, FormatType.Simple);
                var fines = tuple.Item2;
                var finesFormatted = fines
                            .Select(fine => _fineFormatterFactory
                                .CreateFormatter(fine, FormatType.Simple))
                            .ToList();
                ResultRenderer.RenderResultWithListOf(patronFormatted, finesFormatted);
            }
        }
        else
        {
            _messageRenderer.RenderInfoMessage("the patron has no fines");
        }
    }

}
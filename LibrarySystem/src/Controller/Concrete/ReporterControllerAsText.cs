namespace LibrarySystem;

public class ReporterControllerAsText : IExecutableHandler<string>
{
    private Reporter _reporter;
    private StatisticsGenerator _statisticsGenerator;
    private IPatronRepository _patronRepository;
    private IMessageRenderer _messageRenderer;
    private IResultRenderer<Book> _bookRenderer;
    private IResultRenderer<Patron> _patronRenderer;
    private IResultRenderer<Loan> _loanRenderer;
    private EntitySelectorByConsole<Patron> _patronSelector;


    public ReporterControllerAsText(Reporter reporter, StatisticsGenerator statisticsGenerator, IPatronRepository patronRepository, IResultRenderer<Book> bookRenderer, IResultRenderer<Patron> patronRenderer, IResultRenderer<Loan> loanRenderer, IMessageRenderer messageRenderer, EntitySelectorByConsole<Patron> patronSelector)
    {
        _reporter = reporter;
        _statisticsGenerator = statisticsGenerator;
        _patronRepository = patronRepository;

        _bookRenderer = bookRenderer;
        _patronRenderer = patronRenderer;
        _loanRenderer = loanRenderer;
        _messageRenderer = messageRenderer;
        _patronSelector = patronSelector;
    }

    public void Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "show overdue books":
                ShowOverdueBooks();
                break;
            case "show borrowed books":
                ShowCurrentlyBorrowedBooks();
                break;
            case "show current loans by patron":
                ShowCurrentLoansByPatron();
                break;
            case "show loans by patron":
                ShowLoansByPatron();
                break;
            case "show most borrowed books":
                ShowMostBorrowedBooks();
                break;
            case "show most active patrons":
                ShowMostActivePatrons();
                break;
            case "show patron debts":
                ShowPatronsFines();
                break;
            default:
                _messageRenderer.RenderErrorMessage("option not found");
                break;
        }
    }

    public void ShowOverdueBooks()
    {
        var books = _reporter.GetOverdueBooks();
        _bookRenderer.RenderResults(books);
    }

    public void ShowCurrentlyBorrowedBooks()
    {
        var books = _reporter.GetCurrentlyBorrowedBooks();
        _bookRenderer.RenderResults(books);
    }

    public void ShowCurrentLoansByPatron()
    {
        var allPatrons = _reporter.GetPatternsThatBorrowedBooks()
                                .GroupBy(patron => patron.Id)
                                .Select(group => group.First())
                                .ToList();
        var patron = _patronSelector.TryToSelectAtLeastOne(allPatrons);
        if (patron is not null)
        {
            var loans = _reporter.GetLoansByPatron(patron);
            _loanRenderer.RenderResults(loans);
        }
    }

    public void ShowLoansByPatron()
    {
        var allPatrons = _patronRepository.GetAll();
        var patron = _patronSelector.TryToSelectAtLeastOne(allPatrons);
        if (patron is not null)
        {
            var loans = _reporter.GetLoansByPatron(patron);
            if (loans.Any())
            {
                _loanRenderer.RenderResults(loans);
            }
            else
            {
                _messageRenderer.RenderInfoMessage("without history...");
            }
        }
    }

    public void ShowMostBorrowedBooks()
    {
        var books = _statisticsGenerator.GetMostBorrowedBooks();
        _bookRenderer.RenderResults(books);
    }

    public void ShowMostActivePatrons()
    {
        var patrons = _statisticsGenerator.GetMostActivePatrons();
        _patronRenderer.RenderResults(patrons);
    }

    public void ShowPatronsFines()
    {
        var patronsFines = _statisticsGenerator.GetPatronsFines();
        if (patronsFines.Any())
        {

            foreach (var tuple in patronsFines)
            {
                var patron = tuple.Item1;
                var fines = String.Join(", ", tuple.Item2);
                _patronRenderer.RenderResultWith(patron, fines);
            }
        }
        else
        {
            _messageRenderer.RenderInfoMessage("the patron has no fines");
        }
    }

}
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
        _bookRenderer.RenderResults(books);
    }

    public async Task ShowCurrentlyBorrowedBooks()
    {
        var books = await _reporter.GetCurrentlyBorrowedBooks();
        _bookRenderer.RenderResults(books);
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
            _loanRenderer.RenderResults(loans);
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
                _loanRenderer.RenderResults(loans);
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
        _bookRenderer.RenderResults(books);
    }

    public async Task ShowMostActivePatrons()
    {
        var patrons = await _statisticsGenerator.GetMostActivePatrons();
        _patronRenderer.RenderResults(patrons);
    }

    public async Task ShowPatronsFines()
    {
        var patronsFines = await _statisticsGenerator.GetPatronsFines();
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
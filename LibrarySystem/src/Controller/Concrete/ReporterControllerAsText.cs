using Spectre.Console;

namespace LibrarySystem;

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
        await PaginateBooks(async () => await _reporter.GetOverdueBooks());
    }

    public async Task ShowCurrentlyBorrowedBooks()
    {
        await PaginateBooks(async () => await _reporter.GetCurrentlyBorrowedBooks());
    }

    public async Task ShowCurrentLoansByPatron()
    {
        var allPatrons = (await _reporter.GetPatternsThatBorrowedBooks())
                            .GroupBy(patron => patron.Id)
                            .Select(group => group.First())
                            .ToList();
        var patron = await _patronSelector.TryToSelectAtLeastOne(allPatrons);
        if (patron is not null)
        {
            await PaginateLoans(async () => await _reporter.GetLoansByPatron(patron));
        }
    }

    public async Task ShowLoansByPatron()
    {
        var allPatrons = await _patronRepository.GetAll();
        var patron = await _patronSelector.TryToSelectAtLeastOne(allPatrons.ToList());
        if (patron is not null)
        {
            var loans = await _reporter.GetLoansByPatron(patron);
            if (loans.Any())
            {
                await PaginateLoans(async () => loans);
            }
            else
            {
                _messageRenderer.RenderInfoMessage("without history...");
            }
        }
    }

    public async Task ShowMostBorrowedBooks()
    {
        await PaginateBooks(async () => await _statisticsGenerator.GetMostBorrowedBooks());
    }

    public async Task ShowMostActivePatrons()
    {
        var patrons = await _statisticsGenerator.GetMostActivePatrons();
        var patronsFormatted = await Task.WhenAll(patrons.Select(async patron =>
                                await _patronFormatterFactory
                                .CreateVerboseFormatter(patron)));
        ResultRenderer.RenderResults(patronsFormatted.ToList());
    }

    public async Task ShowPatronsFines()
    {
        var patronsFines = await _statisticsGenerator.GetPatronsFines();
        if (patronsFines.Any())
        {
            foreach (var tuple in patronsFines)
            {
                var patron = tuple.Item1;
                var patronFormatted = await _patronFormatterFactory.CreateVerboseFormatter(patron);
                var fines = tuple.Item2;
                var finesFormatted = await Task.WhenAll(fines.Select(async fine =>
                                    await _fineFormatterFactory
                                    .CreateVerboseFormatter(fine)));

                ResultRenderer.RenderResultWithListOf(patronFormatted, finesFormatted.ToList());
            }
        }
        else
        {
            _messageRenderer.RenderInfoMessage("the patron has no fines");
        }
    }

    private async Task PaginateBooks(Func<Task<List<Book>>> getBooks)
    {
        var allBooks = await getBooks();
        int pageSize = 3; // Tamaño de página
        int currentPage = 0;
        bool exit = false;

        while (!exit)
        {
            // Guardar la posición actual del cursor
            var top = Console.CursorTop;

            // Obtener los libros para la página actual
            var pageBooks = allBooks.Skip(currentPage * pageSize).Take(pageSize).ToList();

            // Imprimir la página actual
            AnsiConsole.Write(new Markup($"[bold underline]Page {currentPage + 1}[/]\n\n"));
            await RenderVerboseBooksFormatted(pageBooks);

            // Mostrar opciones de navegación
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(new[] { "Next", "Previous", "Exit" }));

            switch (choice)
            {
                case "Next":
                    if ((currentPage + 1) * pageSize < allBooks.Count)
                    {
                        AnsiConsole.Clear();
                        currentPage++;
                    }
                    else
                    {
                        AnsiConsole.Clear();
                    }
                    break;
                case "Previous":
                    if (currentPage > 0)
                    {
                        AnsiConsole.Clear();
                        currentPage--;
                    }
                    else
                    {
                        AnsiConsole.Clear();
                    }
                    break;
                case "Exit":
                    exit = true;
                    break;
            }
        }
    }

    private async Task PaginateLoans(Func<Task<List<Loan>>> getLoans)
    {
        var allLoans = await getLoans();
        int pageSize = 3; // Tamaño de página
        int currentPage = 0;
        bool exit = false;

        while (!exit)
        {
            // Guardar la posición actual del cursor
            var top = Console.CursorTop;

            // Obtener los préstamos para la página actual
            var pageLoans = allLoans.Skip(currentPage * pageSize).Take(pageSize).ToList();

            // Imprimir la página actual
            AnsiConsole.Write(new Markup($"[bold underline]Page {currentPage + 1}[/]\n\n"));
            await RenderLoansFormatted(pageLoans);

            // Mostrar opciones de navegación
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(new[] { "Next", "Previous", "Exit" }));

            switch (choice)
            {
                case "Next":
                    if ((currentPage + 1) * pageSize < allLoans.Count)
                    {
                        AnsiConsole.Clear();
                        currentPage++;
                    }
                    else
                    {
                        AnsiConsole.Clear();
                    }
                    break;
                case "Previous":
                    if (currentPage > 0)
                    {
                        AnsiConsole.Clear();
                        currentPage--;
                    }
                    else
                    {
                        AnsiConsole.Clear();
                    }
                    break;
                case "Exit":
                    exit = true;
                    break;
            }
        }
    }

    private async Task RenderVerboseBooksFormatted(List<Book> books)
    {
        var booksFormatted = await Task.WhenAll(books.Select(async book =>
            await _bookFormatterFactory.CreateVerboseFormatter(book)));

        ResultRenderer.RenderResults(booksFormatted.ToList());
    }

    private async Task RenderLoansFormatted(List<Loan> loans)
    {
        var loansFormatted = await Task.WhenAll(loans.Select(async loan =>
            await _loanFormatterFactory.CreateVerboseFormatter(loan)));
        ResultRenderer.RenderResults(loansFormatted.ToList());
    }

}
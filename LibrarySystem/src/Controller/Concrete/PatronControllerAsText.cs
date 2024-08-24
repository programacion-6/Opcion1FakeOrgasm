using Spectre.Console;

namespace LibrarySystem;

public class PatronControllerAsText : IExecutableHandler<string>
{
    private IPatronRepository _repository;
    private IEntityCreator<Patron, string> _patronCreator;
    private IEntityUpdater<Patron, string> _patronUpdater;
    private IEntityEliminator<Patron, string> _patronEliminator;
    private IMessageRenderer _messageRenderer;
    private IEntityFormatterFactory<Patron> _patronFormatterFactory;

    public PatronControllerAsText(IPatronRepository repository, IEntityCreator<Patron, string> patronCreator, IEntityUpdater<Patron, string> patronUpdater, IEntityEliminator<Patron, string> patronEliminator, IMessageRenderer messageRenderer, IEntityFormatterFactory<Patron> patronFormatterFactory)
    {
        _repository = repository;
        _patronCreator = patronCreator;
        _patronUpdater = patronUpdater;
        _patronEliminator = patronEliminator;
        _messageRenderer = messageRenderer;
        _patronFormatterFactory = patronFormatterFactory;
    }

    public async Task Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "new":
                await _patronCreator.TryToCreateEntity();
                break;
            case "delete":
                await _patronEliminator.TryToDeleteEntity();
                break;
            case "update":
                await _patronUpdater.TryToUpdateEntity();
                break;
            case "show all":
                await ShowAll();
                break;
            case "find by name":
                await FindPatronByName();
                break;
            case "find by m-number":
                await FindPatronByMembershipNumber();
                break;
            default:
                _messageRenderer.RenderErrorMessage("option not found");
                break;
        }
    }

    private async Task ShowAll()
    {
        await PaginatePatrons(async () => (await _repository.GetAll()).ToList());
    }

    private async Task FindPatronByName()
    {
        var name = AnsiConsole.Ask<string>("Enter the [bold]name[/]:");
        var patronFound = await _repository.GetByName(name);
        var bookFormated = await _patronFormatterFactory.CreateVerboseFormatter(patronFound);
        ResultRenderer.RenderResult(bookFormated);
    }

    private async Task FindPatronByMembershipNumber()
    {
        int membershipNumber = AnsiConsole.Ask<int>("Enter the [bold]membership number[/]:");
        var patronFound = await _repository.GetByMembershipNumber(membershipNumber);
        var bookFormated = await _patronFormatterFactory.CreateVerboseFormatter(patronFound);
        ResultRenderer.RenderResult(bookFormated);
    }

    private async Task PaginatePatrons(Func<Task<List<Patron>>> getPatrons)
    {
        var allPatrons = await getPatrons();
        int pageSize = 3;  // Define el tamaño de página
        int currentPage = 0;
        bool exit = false;

        while (!exit)
        {
            // Guardar la posición actual del cursor
            var top = Console.CursorTop;

            // Obtener los patrones para la página actual
            var pagePatrons = allPatrons.Skip(currentPage * pageSize).Take(pageSize).ToList();

            // Imprimir la página actual
            AnsiConsole.Write(new Markup($"[bold underline]Page {currentPage + 1}[/]\n\n"));
            await RenderVerbosePatronsFormatted(pagePatrons);

            // Mostrar opciones de navegación
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(new[] { "Next", "Previous", "Exit" }));

            switch (choice)
            {
                case "Next":
                    if ((currentPage + 1) * pageSize < allPatrons.Count)
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

    private async Task RenderVerbosePatronsFormatted(List<Patron> patrons)
    {
        var patronsFormatted = await Task.WhenAll(patrons.Select(async patron =>
                                    await _patronFormatterFactory
                                    .CreateVerboseFormatter(patron)));
        ResultRenderer.RenderResults(patronsFormatted.ToList());
    }

}
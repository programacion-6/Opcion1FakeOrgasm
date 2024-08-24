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
    private EntityRendererAsConsolePages<Patron> _patronRendererAsPages;

    public PatronControllerAsText(IPatronRepository repository, IEntityCreator<Patron, string> patronCreator, IEntityUpdater<Patron, string> patronUpdater, IEntityEliminator<Patron, string> patronEliminator, IMessageRenderer messageRenderer, IEntityFormatterFactory<Patron> patronFormatterFactory, EntityRendererAsConsolePages<Patron> patronRendererAsPages)
    {
        _repository = repository;
        _patronCreator = patronCreator;
        _patronUpdater = patronUpdater;
        _patronEliminator = patronEliminator;
        _messageRenderer = messageRenderer;
        _patronFormatterFactory = patronFormatterFactory;
        _patronRendererAsPages = patronRendererAsPages;
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
                await _patronRendererAsPages.RenderByPagination();
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

    private async Task RenderVerbosePatronsFormatted(List<Patron> patrons)
    {
        var patronsFormated = await Task.WhenAll(patrons.Select(async patron =>
                                    await _patronFormatterFactory
                                    .CreateVerboseFormatter(patron)));
        ResultRenderer.RenderResults(patronsFormated.ToList());
    }

}
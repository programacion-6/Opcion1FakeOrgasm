namespace LibrarySystem.Controller.PatronManagement;

public class PatronControllerAsText : IExecutableHandler<string>
{
    private IMessageRenderer _messageRenderer;
    private IEntityFormatterFactory<Patron> _patronFormatterFactory;
    private EntityRendererAsConsolePages<Patron> _patronRendererAsPages;

    private PatronInterfaceHandler _interfaceHandler;
    private PatronSearcher _searcher;

    public PatronControllerAsText(IMessageRenderer messageRenderer,
        IEntityFormatterFactory<Patron> patronFormatterFactory,
        PatronInterfaceHandler interfaceHandler,
        PatronSearcher searcher,
        EntityRendererAsConsolePages<Patron> patronRendererAsPages)
    {
        _messageRenderer = messageRenderer;
        _patronFormatterFactory = patronFormatterFactory;
        _interfaceHandler = interfaceHandler;
        _searcher = searcher;
        _patronRendererAsPages = patronRendererAsPages;
    }

    public async Task Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "new":
                await _interfaceHandler.HandleCreateEntity();
                break;
            case "delete":
                await _interfaceHandler.HandleDeleteEntity();
                break;
            case "update":
                await _interfaceHandler.HandleUpdateEntity();
                break;
            case "show all":
                await _patronRendererAsPages.RenderByPagination();
                break;
            case "find by name":
                await _searcher.FindPatronByName();
                break;
            case "find by m-number":
                await _searcher.FindPatronByMembershipNumber();
                break;
            default:
                _messageRenderer.RenderErrorMessage("option not found");
                break;
        }
    }

    private async Task RenderVerbosePatronsFormatted(List<Patron> patrons)
    {
        var patronsFormated = await Task.WhenAll(patrons.Select(async patron =>
                                    await _patronFormatterFactory
                                    .CreateVerboseFormatter(patron)));
        ResultRenderer.RenderResults(patronsFormated.ToList());
    }
}

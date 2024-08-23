namespace LibrarySystem;

public class PatronControllerAsText : IExecutableHandler<string>
{
    private IReceiver<string> _receiver;
    private IPatronRepository _repository;
    private IEntityCreator<Patron, string> _patronCreator;
    private IEntityUpdater<Patron, string> _patronUpdater;
    private IEntityEliminator<Patron, string> _patronEliminator;
    private IMessageRenderer _messageRenderer;
    private IEntityFormatterFactory<Patron> _patronFormatterFactory;

    public PatronControllerAsText(IPatronRepository repository, IEntityCreator<Patron, string> patronCreator, IEntityUpdater<Patron, string> patronUpdater, IEntityEliminator<Patron, string> patronEliminator, IReceiver<string> receiver, IMessageRenderer messageRenderer, IEntityFormatterFactory<Patron> patronFormatterFactoryr)
    {
        _repository = repository;
        _patronCreator = patronCreator;
        _patronUpdater = patronUpdater;
        _patronEliminator = patronEliminator;
        _receiver = receiver;
        _messageRenderer = messageRenderer;
        _patronFormatterFactory = patronFormatterFactoryr;
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
        var allPatrons = await _repository.GetAll();
        await RenderVerbosePatronsFormatted(allPatrons.ToList());
    }

    private async Task FindPatronByName()
    {
        _messageRenderer.RenderSimpleMessage("Enter the name:");
        var name = _receiver.ReceiveInput();
        var patronFound = await _repository.GetByName(name);
        var bookFormated = await _patronFormatterFactory.CreateVerboseFormatter(patronFound);
        ResultRenderer.RenderResult(bookFormated);
    }

    private async Task FindPatronByMembershipNumber()
    {
        _messageRenderer.RenderSimpleMessage("Enter the membership number:");
        var input = _receiver.ReceiveInput();
        if (!int.TryParse(input, out int membershipNumber))
        {
            _messageRenderer.RenderErrorMessage("invalid input");
        }
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
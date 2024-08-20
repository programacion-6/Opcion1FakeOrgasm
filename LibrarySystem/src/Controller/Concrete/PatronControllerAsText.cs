namespace LibrarySystem;

public class PatronControllerAsText : IExecutableHandler<string>
{
    private IReceiver<string> _receiver;
    private IPatronRepository _repository;
    private IEntityCreator<Patron, string> _patronCreator;
    private IEntityUpdater<Patron, string> _patronUpdater;
    private IEntityEliminator<Patron, string> _patronEliminator;
    private IMessageRenderer _messageRenderer;
    private RendererResultsAsText<Patron> _patronRenderer;

    public PatronControllerAsText(IPatronRepository repository, IEntityCreator<Patron, string> patronCreator, IEntityUpdater<Patron, string> patronUpdater, IEntityEliminator<Patron, string> patronEliminator, IReceiver<string> receiver, IMessageRenderer messageRenderer, RendererResultsAsText<Patron> patronRenderer)
    {
        _repository = repository;
        _patronCreator = patronCreator;
        _patronUpdater = patronUpdater;
        _patronEliminator = patronEliminator;
        _receiver = receiver;
        _messageRenderer = messageRenderer;
        _patronRenderer = patronRenderer;
    }

    public void Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "new":
                _patronCreator.TryToCreateEntity();
                break;
            case "delete":
                _patronEliminator.TryToDeleteEntity();
                break;
            case "update":
                _patronUpdater.TryToUpdateEntity();
                break;
            case "show all":
                _patronRenderer.RenderResults(_repository.GetAll());
                break;
            case "find by name":
                FindPatronByName();
                break;
            case "find by m-number":
                FindPatronByMembershipNumber();
                break;
            default:
                _messageRenderer.RenderErrorMessage("option not found");
                break;
        }
    }

    private void FindPatronByName()
    {
        _messageRenderer.RenderSimpleMessage("Enter the name:");
        var name = _receiver.ReceiveInput();
        var patronFound = _repository.GetByName(name);
        _patronRenderer.RenderResult(patronFound);
    }

    private void FindPatronByMembershipNumber()
    {
        _messageRenderer.RenderSimpleMessage("Enter the membership number:");
        var input = _receiver.ReceiveInput();
        if (!int.TryParse(input, out int membershipNumber))
        {
            _messageRenderer.RenderErrorMessage("invalid input");
        }
        var patronFound = _repository.GetByMembershipNumber(membershipNumber);
        _patronRenderer.RenderResult(patronFound);

    }

}
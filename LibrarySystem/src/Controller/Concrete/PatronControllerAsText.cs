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
                ShowAll();
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

    private void ShowAll()
    {
        var allPatrons = _repository.GetAll();
        RenderPatronsFound(allPatrons);
    }

    private void FindPatronByName()
    {
        _messageRenderer.RenderSimpleMessage("Enter the name:");
        var name = _receiver.ReceiveInput();
        var patronFound = _repository.GetByName(name);
        RenderPatronFound(patronFound);
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
        RenderPatronFound(patronFound);
    }

    private void RenderPatronFound(Patron? bookFound)
    {
        if (bookFound is not null)
        {
            var bookFormated = _patronFormatterFactory.CreateFormatter(bookFound, FormatType.Detailed);
            ResultRenderer.RenderResult(bookFormated);
        }
        else
        {
            ResultRenderer.RenderResult(bookFound);
        }
    }

    private void RenderPatronsFound(List<Patron> booksFound)
    {
        var booksFormated = booksFound.Select
            (book => _patronFormatterFactory
                    .CreateFormatter(book, FormatType.Simple))
                    .ToList();
        ResultRenderer.RenderResults(booksFormated);
    }

}
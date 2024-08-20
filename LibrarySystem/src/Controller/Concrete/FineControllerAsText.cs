namespace LibrarySystem;

public class FineControllerAsText : IExecutableHandler<string>
{
    private DebtManager _debtManager;
    private IFineRepository _fineRepository;
    private IMessageRenderer _messageRenderer;
    private IResultRenderer<Fine> _fineRenderer;
    private EntitySelectorByConsole<Fine> _fineSelector;

    public FineControllerAsText(DebtManager debtManager, IFineRepository fineRepository, IMessageRenderer messageRenderer, IResultRenderer<Fine> fineRenderer, EntitySelectorByConsole<Fine> fineSelector)
    {
        _debtManager = debtManager;
        _fineRepository = fineRepository;
        _messageRenderer = messageRenderer;
        _fineRenderer = fineRenderer;
        _fineSelector = fineSelector;
    }

    public void Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "pay":
                MarkAsPaid();
                break;
            case "make":
                _debtManager.CreateDebtsAutomatically();
                break;
            case "show all":
                ShowFines();
                break;
            case "show actives":
                ShowActiveFines();
                break;
            default:
                _messageRenderer.RenderErrorMessage("option not found");
                break;
        }
    }

    public void ShowFines()
    {
        var fines = _fineRepository.GetAll();
        _fineRenderer.RenderResults(fines);
    }

    public void ShowActiveFines()
    {
        var fines = _fineRepository.GetActiveFines();
        _fineRenderer.RenderResults(fines);
    }

    private void MarkAsPaid()
    {
        var activeFines = _fineRepository.GetActiveFines();
        Fine? fine = _fineSelector.TryToSelectAtLeastOne(activeFines);
        if (fine is not null)
        {
            _debtManager.MarkAsPaid(fine);
            _messageRenderer.RenderSuccessMessage("debt paid");
        }
    }
}
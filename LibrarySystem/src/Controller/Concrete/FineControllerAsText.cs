namespace LibrarySystem;

public class FineControllerAsText : IExecutableHandler<string>
{
    private DebtManager _debtManager;
    private IFineRepository _fineRepository;
    private IMessageRenderer _messageRenderer;
    private IEntityFormatterFactory<Fine> _fineFormatterFactory;
    private EntitySelectorByConsole<Fine> _fineSelector;

    public FineControllerAsText(DebtManager debtManager, IFineRepository fineRepository, IMessageRenderer messageRenderer, IEntityFormatterFactory<Fine> fineFormatterFactory, EntitySelectorByConsole<Fine> fineSelector)
    {
        _debtManager = debtManager;
        _fineRepository = fineRepository;
        _messageRenderer = messageRenderer;
        _fineFormatterFactory = fineFormatterFactory;
        _fineSelector = fineSelector;
    }

    public async Task Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "pay":
                await MarkAsPaid();
                break;
            case "make":
                _debtManager.CreateDebtsAutomatically();
                break;
            case "show all":
                await ShowFines();
                break;
            case "show actives":
                await ShowActiveFines();
                break;
            default:
                _messageRenderer.RenderErrorMessage("option not found");
                break;
        }
    }

    private async Task ShowFines()
    {
        var fines = await _fineRepository.GetAll();
        await RenderVerboseFinesFormatted(fines.ToList());
    }

    private async Task ShowActiveFines()
    {
        var fines = await _fineRepository.GetActiveFines();
        await RenderVerboseFinesFormatted(fines.ToList());
    }

    private async Task MarkAsPaid()
    {
        var activeFines = await _fineRepository.GetActiveFines();
        Fine? fine = await _fineSelector.TryToSelectAtLeastOne(activeFines.ToList());
        if (fine is not null)
        {
            await _debtManager.MarkAsPaid(fine);
            _messageRenderer.RenderSuccessMessage("debt paid");
        }
    }

    private async Task RenderVerboseFinesFormatted(List<Fine> fines)
    {
        var finesFormatted = await Task.WhenAll(fines.Select(async fine =>
                                    await _fineFormatterFactory
                                    .CreateVerboseFormatter(fine)));

        ResultRenderer.RenderResults(finesFormatted.ToList());
    }
}
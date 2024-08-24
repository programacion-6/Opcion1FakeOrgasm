using Spectre.Console;

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
        var allFines = await _fineRepository.GetAll();
        await PaginateFines(allFines.ToList());
    }

    private async Task ShowActiveFines()
    {
        var activeFines = await _fineRepository.GetActiveFines();
        await RenderVerboseFinesFormatted(activeFines.ToList());
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

    private async Task PaginateFines(List<Fine> allFines)
    {
        int pageSize = 3; // Tamaño de página
        int currentPage = 0;
        bool exit = false;

        while (!exit)
        {
            // Guardar la posición actual del cursor
            var top = Console.CursorTop;

            // Obtener las multas para la página actual
            var pageFines = allFines.Skip(currentPage * pageSize).Take(pageSize).ToList();

            // Imprimir la página actual
            AnsiConsole.Write(new Markup($"[bold underline]Page {currentPage + 1}[/]\n\n"));
            await RenderVerboseFinesFormatted(pageFines);

            // Mostrar opciones de navegación
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(new[] { "Next", "Previous", "Exit" }));

            switch (choice)
            {
                case "Next":
                    if ((currentPage + 1) * pageSize < allFines.Count)
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

    private async Task RenderVerboseFinesFormatted(List<Fine> fines)
    {
        var finesFormatted = await Task.WhenAll(fines.Select(async fine =>
                                        await _fineFormatterFactory
                                        .CreateVerboseFormatter(fine)));

        ResultRenderer.RenderResults(finesFormatted.ToList());
    }
}

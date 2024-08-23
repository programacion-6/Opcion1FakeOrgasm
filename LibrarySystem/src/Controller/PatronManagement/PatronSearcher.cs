using Spectre.Console;
namespace LibrarySystem.Controller.PatronManagement;

public class PatronSearcher
{
    private IPatronRepository _repository;
    private IEntityFormatterFactory<Patron> _patronFormatterFactory;

    public PatronSearcher(IPatronRepository repository, IEntityFormatterFactory<Patron> patronFormatterFactory)
    {
        _repository = repository;
        _patronFormatterFactory = patronFormatterFactory;
    }

    public async Task FindPatronByName()
    {
        var name = AnsiConsole.Ask<string>("Enter the [bold]name[/]:");
        var patronFound = await _repository.GetByName(name);
        var bookFormated = await _patronFormatterFactory.CreateVerboseFormatter(patronFound);
        ResultRenderer.RenderResult(bookFormated);
    }

    public async Task FindPatronByMembershipNumber()
    {
        int membershipNumber = AnsiConsole.Ask<int>("Enter the [bold]membership number[/]:");
        var patronFound = await _repository.GetByMembershipNumber(membershipNumber);
        var bookFormated = await _patronFormatterFactory.CreateVerboseFormatter(patronFound);
        ResultRenderer.RenderResult(bookFormated);
    }
}

using Spectre.Console;

namespace LibrarySystem;

public class PatronRequesterByConsole : IEntityRequester<Patron>
{
    private IMessageRenderer _renderer;
    private ErrorLogger _errorLogger;

    public PatronRequesterByConsole(IMessageRenderer renderer)
    {
        _renderer = renderer;
        _errorLogger = new ErrorLogger();
    }

    public Patron? AskForEntity()
    {
        Patron? requestedPatron = null;
        try
        {
            var patronToValidate = ReceivePatronByConsole();
            PatronValidator.ValidatePatron(patronToValidate);
            requestedPatron = patronToValidate;
        }
        catch (PatronException ex)
        {
            _errorLogger.LogErrorBasedOnSeverity(ex.Severity, ex.Message, ex);
            _renderer.RenderErrorMessage($"{ex.Message} \n...{ex.ResolutionSuggestion}");
        }
        catch (Exception ex)
        {
            _errorLogger.LogErrorBasedOnSeverity(SeverityLevel.High, "", ex);
            _renderer.RenderErrorMessage(ex.Message);
        }

        return requestedPatron;
    }

    private Patron ReceivePatronByConsole()
    {
        var name = AnsiConsole.Ask<string>("Enter the [bold]name[/]:");
        var contactDetails = AnsiConsole.Ask<string>("Enter the [bold]contact details[/]:");
        var membershipNumber = AnsiConsole.Ask<int>("Enter the [bold]membership number[/]:");

        var patronReceived = new Patron
        {
            Id = Guid.NewGuid(),
            Name = name,
            MembershipNumber = membershipNumber,
            ContactDetails = contactDetails
        };

        return patronReceived;
    }
}

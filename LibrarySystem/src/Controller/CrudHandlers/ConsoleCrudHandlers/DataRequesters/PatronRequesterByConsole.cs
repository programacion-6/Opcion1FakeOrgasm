namespace LibrarySystem;

public class PatronRequesterByConsole : IEntityRequester<Patron>
{

    private AbstractMessageRenderer _renderer;

    private IReceiver<string> _receiver;

    public PatronRequesterByConsole(IReceiver<string> receiver, AbstractMessageRenderer renderer)
    {
        _receiver = receiver;
        _renderer = renderer;
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
        catch (BookException ex)
        {
            _renderer.RenderErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            _renderer.RenderErrorMessage(ex.Message);
        }

        return requestedPatron;
    }

    private Patron ReceivePatronByConsole()
    {
        _renderer.RenderSimpleMessage("Enter the name: ");
        var name = _receiver.ReceiveInput();
        _renderer.RenderSimpleMessage("Enter the contact details: ");
        var contactDetails = _receiver.ReceiveInput();
        var membershipNumber = GetMembershipNumberAsNumber();

        var patronReceived = new Patron
        {
            Id = Guid.NewGuid(),
            Name = name,
            MembershipNumber = membershipNumber,
            ContactDetails = contactDetails
        };
        return patronReceived;
    }

    private int GetMembershipNumberAsNumber()
    {
        int membershipNumber = -1;
        do
        {
            _renderer.RenderSimpleMessage("Enter the membership number: ");
            var numberAsText = _receiver.ReceiveInput();
            if (int.TryParse(numberAsText, out int number))
            {
                membershipNumber = number;
            }
        } while (membershipNumber == -1);

        return membershipNumber;
    }
}

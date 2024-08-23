namespace LibrarySystem;

public class VerbosePatronFormatter : IEntityFormatter<Patron>
{
    private readonly Patron _entity;

    public VerbosePatronFormatter(Patron entity)
    {
        _entity = entity;
    }

    public override string ToString()
    {
        return _entity.Name +
                    "\n\tcontact: " + _entity.ContactDetails +
                    "\n\tmembership number: " + _entity.MembershipNumber;
    }

    public Patron Entity
    {
        get => _entity;
    }
}

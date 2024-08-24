namespace LibrarySystem;

public class VerbosePatronFormatter : IEntityFormatter<Patron>
{
    public VerbosePatronFormatter(Patron entity) : base(entity)
    {
    }

    public override string ToString()
    {
        return $"[bold plum3]{_entity.Name}[/]" +
               $"\n    [bold]Contact:[/] {_entity.ContactDetails}" +
               $"\n    [bold]Membership Number:[/] {_entity.MembershipNumber}\n";
    }
}

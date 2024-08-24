namespace LibrarySystem;

public class SimplePatronDetailsFormatter : IEntityFormatter<Patron>
{
    public SimplePatronDetailsFormatter(Patron entity) : base(entity)
    {
    }

    public override string ToString()
    {
        return $"[bold plum3]{_entity.Name}[/]";
    }
}

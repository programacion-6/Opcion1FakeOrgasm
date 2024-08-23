namespace LibrarySystem;

public class SimplePatronDetailsFormatter : IEntityFormatter<Patron>
{
    private readonly Patron _entity;

    public SimplePatronDetailsFormatter(Patron entity)
    {
        _entity = entity;
    }

    public override string ToString()
    {
        return _entity.Name;
    }

    public Patron Entity
    {
        get => _entity;
    }
}

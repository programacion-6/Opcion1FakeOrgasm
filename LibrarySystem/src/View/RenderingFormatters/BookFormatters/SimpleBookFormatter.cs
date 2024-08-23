namespace LibrarySystem;

public class SimpleBookFormatter : IEntityFormatter<Book>
{
    private readonly Book _entity;

    public SimpleBookFormatter(Book entity)
    {
        _entity = entity;
    }

    public override string ToString()
    {
        return _entity.Title + " | " + _entity.Genre;
    }

    public Book Entity
    {
        get => _entity;
    }
}

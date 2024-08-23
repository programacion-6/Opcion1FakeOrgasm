namespace LibrarySystem;

public class VerboseBookFormatter : IEntityFormatter<Book>
{
    private readonly Book _entity;

    public VerboseBookFormatter(Book entity)
    {
        _entity = entity;
    }

    public override string ToString()
    {
        return _entity.Title +
                       "\n\tauthor: " + _entity.Author +
                       "\n\tISBN: " + _entity.ISBN +
                       "\n\tgenre: " + _entity.Genre +
                       "\n\tyear: " + _entity.PublicationYear;
    }

    public Book Entity
    {
        get => _entity;
    }
}

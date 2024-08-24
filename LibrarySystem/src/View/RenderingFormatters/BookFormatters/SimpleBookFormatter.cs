namespace LibrarySystem;

public class SimpleBookFormatter : IEntityFormatter<Book>
{
    public SimpleBookFormatter(Book entity) : base(entity)
    {
    }

    public override string ToString()
    {
        return $"[bold plum3]{_entity.Title}[/]" + 
                $"[bold] | {_entity.Genre}[/]";
    }
}

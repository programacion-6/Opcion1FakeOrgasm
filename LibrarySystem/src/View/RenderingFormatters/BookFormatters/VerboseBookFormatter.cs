namespace LibrarySystem;

public class VerboseBookFormatter : IEntityFormatter<Book>
{
    public VerboseBookFormatter(Book entity) : base(entity)
    {
    }
    
    public override string ToString()
    {
        return $"[bold plum3]{_entity.Title}[/]\n" +
               $"    [bold]Author:[/] {_entity.Author}\n" +
               $"    [bold]ISBN:[/] {_entity.ISBN}\n" +
               $"    [bold]Genre:[/] {_entity.Genre}\n" +
               $"    [bold]Year:[/] {_entity.PublicationYear}";
    }
}

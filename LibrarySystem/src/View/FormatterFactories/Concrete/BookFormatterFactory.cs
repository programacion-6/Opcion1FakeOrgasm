namespace LibrarySystem;

public class BookFormatterFactory : IEntityFormatterFactory<Book>
{
    public IEntityFormatter<Book> CreateFormatter(Book entity, FormatType formatType)
    {
        return formatType switch
        {
            FormatType.Simple
                => new SimpleBookDetailsFormatter
                { Entity = entity },
            FormatType.Detailed
                => new AllBookDetailsFormatter
                { Entity = entity },
            _ => throw new Exception("No formatter found")
        };
    }
}
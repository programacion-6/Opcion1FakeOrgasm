namespace LibrarySystem;

public class PatronFormatterFactory : IEntityFormatterFactory<Patron>
{
    public IEntityFormatter<Patron> CreateFormatter(Patron entity, FormatType formatType)
    {
        return formatType switch
        {
            FormatType.Simple
                => new SimplePatronDetailsFormatter
                { Entity = entity },
            FormatType.Detailed
                => new AllPatronDetailsFormatter
                { Entity = entity },
            _ => throw new Exception("No formatter found")
        };
    }
}
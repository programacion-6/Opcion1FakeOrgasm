namespace LibrarySystem;

public class FineFormatterFactory : IEntityFormatterFactory<Fine>
{
    public IEntityFormatter<Fine> CreateFormatter(Fine entity, FormatType formatType)
    {
        return formatType switch
        {
            FormatType.Simple
                => new SimpleFineDetailsFormatter
                { Entity = entity },
            FormatType.Detailed
                => new AllFineDetailsFormatter
                { Entity = entity },
            _ => throw new Exception("No formatter found")
        };
    }
}
namespace LibrarySystem;

public class BookFormatterFactory : IEntityFormatterFactory<Book>
{
    public IEntityFormatter<Book>? CreateSimpleFormatter(Book? entity)
    {
        if (entity is not null)
        {
            var formatter = new SimpleBookFormatter(entity);
            
            return formatter;
        }

        return null;
    }

    public Task<IEntityFormatter<Book>?> CreateVerboseFormatter(Book? entity)
    {
        if (entity is not null)
        {
            var formatter = Task.FromResult<IEntityFormatter<Book>?>(
                new VerboseBookFormatter(entity));

            return formatter;
        }

        return Task.FromResult<IEntityFormatter<Book>?>(null);
    }
}

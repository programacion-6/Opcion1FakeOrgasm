namespace LibrarySystem;

public class PatronFormatterFactory : IEntityFormatterFactory<Patron>
{
    public IEntityFormatter<Patron>? CreateSimpleFormatter(Patron? entity)
    {
        if (entity is not null)
        {
            var formatter = new SimplePatronDetailsFormatter(entity);

            return formatter;
        }

        return null;
    }

    public Task<IEntityFormatter<Patron>?> CreateVerboseFormatter(Patron? entity)
    {
        if (entity is not null)
        {
            var formatter = Task.FromResult<IEntityFormatter<Patron>?>(
                new VerbosePatronFormatter(entity));

            return formatter;
        }

        return Task.FromResult<IEntityFormatter<Patron>?>(null);
    }
}
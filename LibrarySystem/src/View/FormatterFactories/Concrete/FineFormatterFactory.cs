namespace LibrarySystem;

public class FineFormatterFactory : IEntityFormatterFactory<Fine>
{
    private readonly ILoanRepository _loanRepository;

    public FineFormatterFactory(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public IEntityFormatter<Fine>? CreateSimpleFormatter(Fine? entity)
    {
        if (entity is not null)
        {
            var formatter = new SimpleFineFormatter(entity);

            return formatter;
        }

        return null;
    }

    public async Task<IEntityFormatter<Fine>?> CreateVerboseFormatter(Fine? entity)
    {
        if (entity is not null)
        {
            var formatter = new VerboseFineFormatter(entity, _loanRepository);
            await formatter.LoadRelatedData();

            return formatter;
        }

        return null;
    }
}
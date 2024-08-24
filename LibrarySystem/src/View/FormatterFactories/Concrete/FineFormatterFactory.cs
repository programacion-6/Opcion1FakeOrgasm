namespace LibrarySystem;

public class FineFormatterFactory : IEntityFormatterFactory<Fine>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;

    public FineFormatterFactory(ILoanRepository loanRepository, IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
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
            var formatter = new VerboseFineFormatter(entity, _loanRepository, _bookRepository, _patronRepository);
            await formatter.LoadRelatedData();

            return formatter;
        }

        return null;
    }
}
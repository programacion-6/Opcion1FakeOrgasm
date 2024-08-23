namespace LibrarySystem;

public class LoanFormatterFactory : IEntityFormatterFactory<Loan>
{
    private IBookRepository _bookRepository;
    private IPatronRepository _patronRepository;

    public LoanFormatterFactory(IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public IEntityFormatter<Loan>? CreateSimpleFormatter(Loan? entity)
    {
        if (entity is not null)
        {
            var formatter = new SimpleLoanFormatter(entity);

            return formatter;
        }

        return null;
    }

    public async Task<IEntityFormatter<Loan>?> CreateVerboseFormatter(Loan? entity)
    {
        if (entity is not null)
        {
            var formatter = new VerboseLoanFormatter(entity, _bookRepository, _patronRepository);
            await formatter.LoadRelatedData();
        }

        return null;
    }

}
namespace LibrarySystem;

public class LoanFormatterFactory : IEntityFormatterFactory<Loan>
{
    public IEntityFormatter<Loan> CreateFormatter(Loan entity, FormatType formatType)
    {
        return formatType switch
        {
            FormatType.Simple
                => new SimpleLoanDetailsFormatter
                { Entity = entity },
            FormatType.Detailed
                => new AllLoanDetailsFormatter
                { Entity = entity },
            _ => throw new Exception("No formatter found")
        };
    }
}
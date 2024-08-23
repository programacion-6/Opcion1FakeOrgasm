namespace LibrarySystem;

public class SimpleLoanFormatter : IEntityFormatter<Loan>
{
    private readonly Loan _entity;

    public SimpleLoanFormatter(Loan entity)
    {
        _entity = entity;
    }

    public override string ToString()
    {
        return "Loan " + (_entity.WasReturn ? "returned" : "active")
                    + "\n\t" + _entity.LoanDate + " - " + _entity.ReturnDate;
    }

    public Loan Entity
    {
        get => _entity;
    }
}
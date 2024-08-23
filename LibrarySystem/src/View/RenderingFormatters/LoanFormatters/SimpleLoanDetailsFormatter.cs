namespace LibrarySystem;

public class SimpleLoanDetailsFormatter : IEntityFormatter<Loan>
{
    public Loan? Entity { get; set; }

    public override string ToString()
    {
        if (Entity is null)
        {
            return string.Empty;
        }

        return "Loan " + (Entity.WasReturn ? "returned" : "active")
            + "\n\t" + Entity.LoanDate + " - " + Entity.ReturnDate;
    }
}
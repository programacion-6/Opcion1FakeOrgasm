namespace LibrarySystem;

public class AllFineDetailsFormatter : IEntityFormatter<Fine>
{
    public Fine? Entity { get; set; }

    public override string ToString()
    {
        if (Entity is null)
        {
            return string.Empty;
        }

        var loanFormatted = new AllLoanDetailsFormatter { Entity = Entity.Loan };

        return "Fine: " + Entity.FineAmount + "$ | "
                    + (Entity.WasPayed ? "paid" : "active")
                    + "\n\t" + loanFormatted;
    }
}
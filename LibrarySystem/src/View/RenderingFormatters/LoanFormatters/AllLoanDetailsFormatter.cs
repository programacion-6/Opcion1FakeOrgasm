namespace LibrarySystem;

public class AllLoanDetailsFormatter : IEntityFormatter<Loan>
{
    public Loan? Entity { get; set; }

    public override string ToString()
    {
        if (Entity is null)
        {
            return string.Empty;
        }

        return "Loan " + (Entity.WasReturn ? "returned" : "active") + " | "
            + Entity.LoanDate + " - " + Entity.ReturnDate
            + "\n\tBook: " + Entity.Book.Title
             + "\n\tPatron: " + Entity.Patron.Name;
    }
}
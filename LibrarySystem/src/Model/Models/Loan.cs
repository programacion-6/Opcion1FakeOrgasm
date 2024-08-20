namespace LibrarySystem;

public class Loan : EntityBase
{
    public required Book Book { get; set; }
    public required Patron Patron { get; set; }
    public DateTime LoanDate { get; set; } = DateTime.Now;
    public required DateTime ReturnDate { get; set; }
    public bool WasReturn { get; set; } = false;

    public override string ToString()
    {
        return "Loan " + (WasReturn ? "returned" : "active") + " | "
            + LoanDate + " - " + ReturnDate
            + "\n\tBook: " + Book.Title
             + "\n\tPatron: " + Patron.Name;
    }
}
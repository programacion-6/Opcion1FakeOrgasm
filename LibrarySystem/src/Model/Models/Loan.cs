namespace LibrarySystem;

public class Loan : EntityBase
{
    public required Guid BookId { get; set; }
    public required Guid PatronId { get; set; }
    public DateTime LoanDate { get; set; } = DateTime.Now;
    public required DateTime ReturnDate { get; set; }
    public bool WasReturn { get; set; } = false;

    public override string ToString()
    {
        return "Loan " + (WasReturn ? "returned" : "active") + " | "
            + LoanDate + " - " + ReturnDate
            + "\n\tBook: " + BookId
             + "\n\tPatron: " + PatronId;
    }
}
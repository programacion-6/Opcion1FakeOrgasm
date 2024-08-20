namespace LibrarySystem;

public class Fine : EntityBase
{
    public required Loan Loan { get; set; }
    public required double FineAmount { get; set; }
    public bool WasPayed { get; set; } = false;

    public override string ToString()
    {
        return "Fine: " + FineAmount + "$ | " 
            + (WasPayed ? "paid" : "active") 
            + "\n\t" + Loan;
    }
}
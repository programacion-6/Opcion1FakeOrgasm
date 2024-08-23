namespace LibrarySystem;

public class Fine : EntityBase
{
    public required Loan Loan { get; set; }
    public required double FineAmount { get; set; }
    public bool WasPayed { get; set; } = false;
}
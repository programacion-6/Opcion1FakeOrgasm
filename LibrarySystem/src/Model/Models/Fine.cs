namespace LibrarySystem;

public class Fine : EntityBase
{
    public required Guid LoanId { get; set; }
    public required double FineAmount { get; set; }
    public bool WasPayed { get; set; } = false;
}
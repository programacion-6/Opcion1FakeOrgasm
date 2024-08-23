namespace LibrarySystem;
public class Fine : EntityBase
{
    private Guid _loanId;
    private double _fineAmount;
    private bool _wasPayed = false;

    public required Guid LoanId
    {
        get => _loanId;
        set => _loanId = value;
    }

    public required double FineAmount
    {
        get => _fineAmount;
        set => _fineAmount = value;
    }

    public required bool WasPayed
    {
        get => _wasPayed;
        set => _wasPayed = value;
    }

    public override string ToString()
    {
        return "Fine: " + FineAmount + "$ | " 
            + (WasPayed ? "paid" : "active") 
            + "\n\t" + LoanId;
    }

}
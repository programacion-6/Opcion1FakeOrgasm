namespace LibrarySystem;

public class VerboseFineFormatter : IEntityVerboseFormatter<Fine>
{
    private readonly Fine _entity;
    private readonly ILoanRepository _loanRepository;
    private Loan? _loan;

    public VerboseFineFormatter(Fine entity, ILoanRepository loanRepository)
    {
        _entity = entity;
        _loanRepository = loanRepository;
    }

    public async Task LoadRelatedData()
    {
        _loan = await _loanRepository.GetById(_entity.LoanId);
    }

    public override string ToString()
    {
        return "Fine: " + _entity.FineAmount + "$ | "
                    + (_entity.WasPayed ? "paid" : "active")
                    + "\n\tLoan:" + GetLoanFormatted();
    }

    private string GetLoanFormatted()
    {
        var loanFormatted = "no loaded";

        if (_loan is not null)
        {
            loanFormatted = new SimpleLoanFormatter(_loan).ToString();
        }

        return loanFormatted;
    }

    public Fine Entity
    {
        get => _entity;
    }
}
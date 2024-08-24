namespace LibrarySystem;

public class VerboseFineFormatter : IEntityVerboseFormatter<Fine>
{
    private readonly ILoanRepository _loanRepository;
    private Loan? _loan;

    public VerboseFineFormatter(Fine entity, ILoanRepository loanRepository) : base(entity)
    {
        _loanRepository = loanRepository;
    }

    private string GetLoanFormatted()
    {
        var loanFormatted = "[lightsteelblue1] no loaded [/]";

        if (_loan is not null)
        {
            loanFormatted = new SimpleLoanFormatter(_loan).ToString();
        }

        return loanFormatted;
    }

    public override async Task LoadRelatedData()
    {
        _loan = await _loanRepository.GetById(_entity.LoanId);

    }

    public override string ToString()
    {
        var statusFineFormatted = _entity.WasPayed ?
            "$ | [bold green] paid [/]" :
            "$ | [bold red] active [/]";

        return $"[bold plum3]Fine:[/]" + statusFineFormatted
        + "\n    [bold]Loan:[/]" + GetLoanFormatted();
    }
}
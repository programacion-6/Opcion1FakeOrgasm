namespace LibrarySystem;

public class VerboseFineFormatter : IEntityVerboseFormatter<Fine>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;
    private IEntityVerboseFormatter<Loan>? _formatter;

    public VerboseFineFormatter(Fine entity, ILoanRepository loanRepository, IBookRepository bookRepository, IPatronRepository patronRepository) : base(entity)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    private string GetLoanFormatted()
    {
        var loanFormatted = "[lightsteelblue1] no loaded [/]";

        if (_formatter is not null)
        {
            loanFormatted = _formatter.ToString();
        }

        return loanFormatted;
    }

    public override async Task LoadRelatedData()
    {
        var _loan = await _loanRepository.GetById(_entity.LoanId);
        _formatter = new VerboseLoanFormatter(_loan, _bookRepository, _patronRepository);
        await _formatter.LoadRelatedData();
    }

    public override string ToString()
    {
        var statusFineFormatted = _entity.WasPayed ?
            $" {_entity.FineAmount} $ | [bold green] paid [/]" :
            $" {_entity.FineAmount} $ | [bold red] active [/]";

        return $"[bold plum3]Fine:[/]" + statusFineFormatted
        + "\n    " + GetLoanFormatted();
    }
}
namespace LibrarySystem;

public class SimpleLoanFormatter : IEntityFormatter<Loan>
{
    public SimpleLoanFormatter(Loan entity) : base(entity)
    {
    }

    public override string ToString()
    {
        var statusFineFormatted = _entity.WasReturn ?
            " | [bold green] returned [/]" :
            " | [bold red] active [/]";

        return "[bold plum3]Loan:[/]" + statusFineFormatted
                    + "\n    " + $"[grey74]{_entity.LoanDate} - {_entity.ReturnDate}[/]";
    }
}
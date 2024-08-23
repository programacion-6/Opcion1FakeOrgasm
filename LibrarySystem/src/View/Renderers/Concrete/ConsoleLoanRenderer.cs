namespace LibrarySystem;

public class ConsoleLoanRenderer : IResultRenderer<Loan>
{
    private readonly LoanFormatter _loanFormatter;

    public ConsoleLoanRenderer(LoanFormatter loanFormatter)
    {
        _loanFormatter = loanFormatter;
    }

    public void RenderResult(Loan? result)
    {
        if (result is not null)
        {
            Console.WriteLine($"{_loanFormatter.FormatLoan(result.Id)}\n");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no loan found");
            Console.WriteLine(infoMessage);
        }
    }

    public void RenderResults(List<Loan> results)
    {
        if (results.Any())
        {
            int index = 0;
            foreach (var result in results)
            {
                Console.WriteLine($"{++index}. {_loanFormatter.FormatLoan(result.Id)}");
            }
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no loans found");
            Console.WriteLine(infoMessage);
        }
    }

    public void RenderResultWith<S>(Loan? result, S someElse)
    {
        if (result is not null)
        {
            Console.WriteLine($"{_loanFormatter.FormatLoan(result.Id)} : {someElse}");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no loans found");
            Console.WriteLine(infoMessage);
        }
    }
}
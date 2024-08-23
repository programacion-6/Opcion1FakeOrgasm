namespace LibrarySystem;

public class ConsoleFineRenderer : IResultRenderer<Fine>
{
    private readonly LoanFormatter _loanFormatter;

    public ConsoleFineRenderer(LoanFormatter loanFormatter)
    {
        _loanFormatter = loanFormatter;
    }

    public void RenderResult(Fine? result)
    {
        if (result is not null)
        {
            var loanFormatted = _loanFormatter.FormatLoan(result.IdLoan);
            Console.WriteLine($"Fine: {result.FineAmount}$ | {(result.WasPayed ? "paid" : "active")}\n{loanFormatted}\n");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no fine found");
            Console.WriteLine(infoMessage);
        }
    }

    public void RenderResults(List<Fine> results)
    {
        if (results.Any())
        {
            int index = 0;
            foreach (var result in results)
            {
                var loanFormatted = _loanFormatter.FormatLoan(result.IdLoan);
                Console.WriteLine($"{++index}. Fine: {result.FineAmount}$ | {(result.WasPayed ? "paid" : "active")}\n{loanFormatted}\n");
            }
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no fines found");
            Console.WriteLine(infoMessage);
        }
    }

    public void RenderResultWith<S>(Fine? result, S someElse)
    {
        if (result is not null)
        {
            var loanFormatted = _loanFormatter.FormatLoan(result.IdLoan);
            Console.WriteLine($"Fine: {result.FineAmount}$ | {(result.WasPayed ? "paid" : "active")}\n{loanFormatted} : {someElse}\n");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no fines found");
            Console.WriteLine(infoMessage);
        }
    }
}
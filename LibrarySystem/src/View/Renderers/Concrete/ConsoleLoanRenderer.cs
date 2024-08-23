namespace LibrarySystem;

public class ConsoleLoanRenderer : IResultRenderer<Loan>
{

    public void RenderResult(Loan? result)
    {
        if (result is not null)
        {
            Console.WriteLine($"{result}\n");
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
                Console.WriteLine($"{++index}. {result}");
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
            Console.WriteLine($"{result} : {someElse}");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no loans found");
            Console.WriteLine(infoMessage);
        }
    }
}
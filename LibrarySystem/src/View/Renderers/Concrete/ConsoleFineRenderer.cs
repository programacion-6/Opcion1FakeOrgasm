namespace LibrarySystem;

public class ConsoleFineRenderer : IResultRenderer<Fine>
{

    public void RenderResult(Fine? result)
    {
        if (result is not null)
        {
            Console.WriteLine($"{result}\n");
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
                Console.WriteLine($"{++index}. {result}");
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
            Console.WriteLine($"{result} : {someElse}");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no fines found");
            Console.WriteLine(infoMessage);
        }
    }
}
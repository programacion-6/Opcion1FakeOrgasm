namespace LibrarySystem;

public class ConsolePatronRenderer : IResultRenderer<Patron>
{
    public void RenderResult(Patron? result)
    {
        if (result is not null)
        {
            Console.WriteLine($"{result}\n");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no patron found");
            Console.WriteLine(infoMessage);
        }
    }

    public void RenderResults(List<Patron> results)
    {
        if (results.Any())
        {
            int index = 0;
            foreach (var result in results)
            {
                Console.WriteLine($"{++index}. {result.Name}");
            }
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no patrons found");
            Console.WriteLine(infoMessage);
        }
    }

    public void RenderResultWith<S>(Patron? result, S someElse)
    {
        if (result is not null)
        {
            Console.WriteLine($"{result.Name} : {someElse}");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no patrons found");
            Console.WriteLine(infoMessage);
        }
    }
}
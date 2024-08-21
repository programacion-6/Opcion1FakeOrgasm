namespace LibrarySystem;

public class ConsoleBookRenderer : IResultRenderer<Book>
{
    public void RenderResult(Book? result)
    {
        if (result is not null)
        {
            Console.WriteLine($"{result}\n");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no book found");
            Console.WriteLine(infoMessage);
        }
    }

    public void RenderResults(List<Book> results)
    {
        if (results.Any())
        {
            int index = 0;
            foreach (var result in results)
            {
                Console.WriteLine($"{++index}. {result.Title}");
            }
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no books found");
            Console.WriteLine(infoMessage);
        }
    }

    public void RenderResultWith<S>(Book? result, S someElse)
    {
        if (result is not null)
        {
            Console.WriteLine($"{result.Title} : {someElse}");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no books found");
            Console.WriteLine(infoMessage);
        }
    }
}
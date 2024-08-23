using Spectre.Console;

namespace LibrarySystem;

public static class ResultRenderer
{
    public static void RenderResult<R>(R? result)
    {
        if (result is not null)
        {
            AnsiConsole.WriteLine($"{result}\n");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no found");
            AnsiConsole.WriteLine(infoMessage);
        }
    }

    public static void RenderResults<R>(List<R> results)
    {
        if (results.Any())
        {
            int index = 0;
            foreach (var result in results)
            {
                AnsiConsole.WriteLine($"{++index}. {result}");
            }
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no results found");
            AnsiConsole.WriteLine(infoMessage);
        }
    }

    public static void RenderResultWith<R, S>(R? result, S someElse)
    {
        if (result is not null)
        {
            AnsiConsole.WriteLine($"{result} : {someElse}");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no found");
            AnsiConsole.WriteLine(infoMessage);
        }
    }

    public static void RenderResultWithListOf<R, S>(R? result, List<S> someElse)
    {
        if (result is not null)
        {
            AnsiConsole.WriteLine($"{result}");
            AnsiConsole.Write("\t");
            RenderResults(someElse);
            AnsiConsole.WriteLine();
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no found");
            AnsiConsole.WriteLine(infoMessage);
        }
    }

}
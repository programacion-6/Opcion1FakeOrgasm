using Spectre.Console;

namespace LibrarySystem;

public static class ResultRenderer
{
    public static void RenderResult<R>(R? result)
    {
        if (result is not null)
        {
            var panel = new Panel(new Markup($"[bold green]{result}[/]"))
            {
                Border = BoxBorder.Rounded,
            };
            AnsiConsole.Write(panel);
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("No result found");
            AnsiConsole.MarkupLine(infoMessage);
        }
    }

    public static void RenderResults<R>(List<R> results)
    {
        if (results.Any())
        {
            int index = 0;
            foreach (var result in results)
            {
                AnsiConsole.MarkupLine($"[bold]{++index}[/]. {result}");
            }
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no results found");
            AnsiConsole.MarkupLine(infoMessage);
        }
    }

    public static void RenderResultWith<R, S>(R? result, S someElse)
    {
        if (result is not null)
        {
            var panel = new Panel(new Markup($"[bold green]{result}[/] : [bold cyan]{someElse}[/]"))
            {
                Border = BoxBorder.Rounded
            };
            AnsiConsole.Write(panel);
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("No result found");
            AnsiConsole.MarkupLine(infoMessage);
        }
    }

    public static void RenderResultWithListOf<R, S>(R? result, List<S> someElse)
    {
        if (result is not null)
        {
            var panel = new Panel(new Markup($"[bold green]{result}[/]"))
            {
                Border = BoxBorder.Rounded,
            };
            AnsiConsole.Write(panel);

            if (someElse.Any())
            {
                var list = new List<string>();
                foreach (var item in someElse)
                {
                    list.Add($"- {item}");
                }

                var listPanel = new Panel(new Markup(string.Join("\n", list)))
                {
                    Border = BoxBorder.Rounded,
                    Header = new PanelHeader("[bold]Details[/]")
                };
                AnsiConsole.Write(listPanel);
            }
            else
            {
                var infoMessage = ConsoleFormatter.AsAnInfo("No additional items found");
                AnsiConsole.MarkupLine(infoMessage);
            }
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("No result found");
            AnsiConsole.MarkupLine(infoMessage);
        }
    }
}

using Spectre.Console;

namespace LibrarySystem;

public class ConsoleMessageRenderer : IMessageRenderer
{
    public void RenderSimpleMessage(string message)
    {
        AnsiConsole.MarkupLine(message);
    }

    public void RenderErrorMessage(string message)
    {
        var noFoundMessage = ConsoleFormatter.AsAnError(message);
        AnsiConsole.MarkupLine(noFoundMessage);
    }

    public void RenderHighlightMessage(string message)
    {
        var highlightMessage = ConsoleFormatter.AsAHighlight(message);
        AnsiConsole.MarkupLine(highlightMessage);
    }

    public void RenderIndicatorMessage(string message)
    {
        var indicatorMessage = ConsoleFormatter.AsIndicator(message);
        AnsiConsole.MarkupLine(indicatorMessage);
    }

    public void RenderInfoMessage(string message)
    {
        var infoMessage = ConsoleFormatter.AsAnInfo(message);
        AnsiConsole.MarkupLine(infoMessage);
    }

    public void RenderSuccessMessage(string message)
    {
        var successMessage = ConsoleFormatter.AsSuccess(message);
        AnsiConsole.MarkupLine(successMessage);
    }
}
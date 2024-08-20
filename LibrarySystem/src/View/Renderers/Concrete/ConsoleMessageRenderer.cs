namespace LibrarySystem;

public class ConsoleMessageRenderer : IMessageRenderer
{
    public void RenderSimpleMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void RenderErrorMessage(string message)
    {
        var noFoundMessage = ConsoleFormatter.AsAnError(message);
        Console.WriteLine(noFoundMessage);
    }

    public void RenderHighlightMessage(string message)
    {
        var highlightMessage = ConsoleFormatter.AsAHighlight(message);
        Console.WriteLine(highlightMessage);
    }

    public void RenderIndicatorMessage(string message)
    {
        var indicatorMessage = ConsoleFormatter.AsIndicator(message);
        Console.WriteLine(indicatorMessage);
    }

    public void RenderInfoMessage(string message)
    {
        var infoMessage = ConsoleFormatter.AsAnInfo(message);
        Console.WriteLine(infoMessage);
    }

    public void RenderSuccessMessage(string message)
    {
        var successMessage = ConsoleFormatter.AsSuccess(message);
        Console.WriteLine(successMessage);
    }
}
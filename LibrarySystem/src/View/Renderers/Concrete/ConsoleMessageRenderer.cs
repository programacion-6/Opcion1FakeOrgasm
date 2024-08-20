namespace LibrarySystem;

public class ConsoleMessageRenderer : AbstractMessageRenderer
{
    public ConsoleMessageRenderer(IViewRenderer<string> renderer) : base(renderer)
    {
    }

    public override void RenderSimpleMessage(string message)
    {
        _renderer.Render(message);
    }

    public override void RenderErrorMessage(string message)
    {
        var noFoundMessage = ConsoleFormatter.AsAnError(message);
        _renderer.Render(noFoundMessage);
    }

    public override void RenderHighlightMessage(string message)
    {
        var highlightMessage = ConsoleFormatter.AsAHighlight(message);
        _renderer.Render(highlightMessage);
    }

    public override void RenderIndicatorMessage(string message)
    {
        var indicatorMessage = ConsoleFormatter.AsIndicator(message);
        _renderer.Render(indicatorMessage);
    }

    public override void RenderInfoMessage(string message)
    {
        var infoMessage = ConsoleFormatter.AsAnInfo(message);
        _renderer.Render(infoMessage);
    }

    public override void RenderSuccessMessage(string message)
    {
        var successMessage = ConsoleFormatter.AsSuccess(message);
        _renderer.Render(successMessage);
    }
}
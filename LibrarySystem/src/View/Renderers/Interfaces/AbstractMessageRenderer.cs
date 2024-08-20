namespace LibrarySystem;

public abstract class AbstractMessageRenderer
{
    protected IViewRenderer<string> _renderer;

    public AbstractMessageRenderer(IViewRenderer<string> renderer)
    {
        _renderer = renderer;
    }

    public abstract void RenderSimpleMessage(string message);
    public abstract void RenderSuccessMessage(string message);
    public abstract void RenderErrorMessage(string message);
    public abstract void RenderInfoMessage(string message);
    public abstract void RenderIndicatorMessage(string message);
    public abstract void RenderHighlightMessage(string message);
}
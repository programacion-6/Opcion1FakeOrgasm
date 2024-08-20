namespace LibrarySystem;

public class RendererResultsAsText<R>
{
    private IViewRenderer<string> _renderer;

    public RendererResultsAsText(IViewRenderer<string> renderer)
    {
        _renderer = renderer;
    }

    public void RenderResult(R? result)
    {
        if (result is not null)
        {
            _renderer.Render($"{result}\n");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no results");
            _renderer.Render(infoMessage);
        }
    }

    public void RenderResults(List<R> results)
    {
        if (results.Any())
        {

            int index = 0;
            foreach (var result in results)
            {
                _renderer.Render($"{++index}. {result}");
            }
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no results");
            _renderer.Render(infoMessage);
        }
    }

    public void RenderResultWith<S>(R? result, S someElse)
    {
        if (result is not null)
        {
            _renderer.Render($"{result} : {someElse}");
        }
        else
        {
            var infoMessage = ConsoleFormatter.AsAnInfo("no results");
            _renderer.Render(infoMessage);
        }

    }
}
namespace LibrarySystem;

public class EntitySelectorByConsole<T> where T : EntityBase
{
    private const int NO_INDEX_FOUND = -1;
    private AbstractMessageRenderer _messageRenderer;
    private RendererResultsAsText<T> _renderer;
    private IReceiver<string> _receiver;

    public EntitySelectorByConsole(RendererResultsAsText<T> renderer, AbstractMessageRenderer messageRenderer, IReceiver<string> receiver)
    {
        _renderer = renderer;
        _messageRenderer = messageRenderer;
        _receiver = receiver;
    }

    public T? TryToSelectAtLeastOne(List<T> entities)
    {
        _messageRenderer.RenderIndicatorMessage("choose one");
        _renderer.RenderResults(entities);

        if (entities.Any())
        {
            T? entitySelected;
            do
            {
                entitySelected = SelectEntityByConsole(entities);
            }
            while (entitySelected is null);
            return entitySelected;
        }

        return null;
    }

    private T? SelectEntityByConsole(List<T> entities)
    {
        var index = GetValidIndex(entities);
        var entitySelected = SelectEntityByIndex(index, entities);
        return entitySelected;
    }

    private int GetValidIndex(List<T> entities)
    {
        var index = NO_INDEX_FOUND;
        _messageRenderer.RenderSimpleMessage("select one by its number, enter it:");
        while (index == NO_INDEX_FOUND)
        {
            var inputReceived = _receiver.ReceiveInput();
            index = TryToParseTheIndex(inputReceived);
            if (index == NO_INDEX_FOUND)
            {
                _messageRenderer.RenderErrorMessage("enter a valid number...");
            }
            else if (!IsValidIndex(--index, entities))
            {
                index = NO_INDEX_FOUND;
                _messageRenderer.RenderErrorMessage("out of range, try again...");
            }
        }

        return index;
    }

    private int TryToParseTheIndex(string inputReceived)
    {
        if (Int32.TryParse(inputReceived, out int indexParsed))
        {
            return indexParsed;
        }
        else
        {
            return NO_INDEX_FOUND;
        }
    }

    private bool IsValidIndex(int index, List<T> items)
    {
        return index >= 0 && index < items.Count;
    }

    private T? SelectEntityByIndex(int index, List<T> items)
    {
        return IsValidIndex(index, items)
        ? items[index] : default;
    }
}

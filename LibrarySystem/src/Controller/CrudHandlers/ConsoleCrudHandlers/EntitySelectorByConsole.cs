using Spectre.Console;

namespace LibrarySystem;

public class EntitySelectorByConsole<T> where T : EntityBase
{
    private IMessageRenderer _messageRenderer;
    protected IEntityFormatterFactory<T> _formatterFactory;

    public EntitySelectorByConsole(IMessageRenderer messageRenderer, IEntityFormatterFactory<T> formatterFactory)
    {
        _messageRenderer = messageRenderer;
        _formatterFactory = formatterFactory;
    }

    public T? TryToSelectAtLeastOne(List<T> entities)
    {
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

    private T SelectEntityByConsole(List<T> entities)
    {
        var selectionPrompt = CreatePrompt(entities);
        var selected = AnsiConsole.Prompt(selectionPrompt);
        _messageRenderer.RenderIndicatorMessage("Selected");
        ResultRenderer.RenderResult(selected);

        return selected.Entity;
    }

    private SelectionPrompt<IEntityFormatter<T>> CreatePrompt(List<T> entities)
    {
        var selectionPrompt = new SelectionPrompt<IEntityFormatter<T>>()
                    .Title("Choose one")
                    .PageSize(5)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .HighlightStyle(new Style(foreground: Color.LightGreen));

        AddChoicesToSelector(selectionPrompt, entities);
        
        return selectionPrompt;
    }

    private void AddChoicesToSelector(SelectionPrompt<IEntityFormatter<T>> selectionPrompt, List<T> entities)
    {
        foreach (var entity in entities)
        {
            var formatter = _formatterFactory.CreateFormatter(entity, FormatType.Simple);
            selectionPrompt.AddChoice(formatter);
        }
    }
}

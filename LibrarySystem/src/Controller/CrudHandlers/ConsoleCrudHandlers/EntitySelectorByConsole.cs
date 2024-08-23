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

    public async Task<T?> TryToSelectAtLeastOne(List<T> entities)
    {
        if (entities.Any())
        {
            T? entitySelected;
            do
            {
                entitySelected = await SelectEntityByConsole(entities);
            }
            while (entitySelected is null);
            return entitySelected;
        }

        return null;
    }

    private async Task<T> SelectEntityByConsole(List<T> entities)
    {
        var selectionPrompt = await CreatePrompt(entities);
        var selected = AnsiConsole.Prompt(selectionPrompt);
        _messageRenderer.RenderIndicatorMessage("Selected");
        ResultRenderer.RenderResult(selected);

        return selected.Entity;
    }

    private async Task<SelectionPrompt<IEntityFormatter<T>>> CreatePrompt(List<T> entities)
    {
        var selectionPrompt = new SelectionPrompt<IEntityFormatter<T>>()
                    .Title("Choose one")
                    .PageSize(5)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .HighlightStyle(new Style(foreground: Color.LightGreen));

        await AddChoicesToSelector(selectionPrompt, entities);

        return selectionPrompt;
    }

    private async Task AddChoicesToSelector(SelectionPrompt<IEntityFormatter<T>> selectionPrompt, List<T> entities)
    {
        foreach (var entity in entities)
        {
            var formatter = await _formatterFactory.CreateVerboseFormatter(entity);
            if (formatter is not null)
            {
                selectionPrompt.AddChoice(formatter);
            }
        }
    }
}

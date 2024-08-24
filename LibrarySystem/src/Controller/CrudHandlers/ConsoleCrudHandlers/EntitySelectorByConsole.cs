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
        var prompt = CreatePrompt(entities);
        var selected = AnsiConsole.Prompt(prompt);
        _messageRenderer.RenderIndicatorMessage("Selected");
        var entitySelectedFormated = await _formatterFactory.CreateVerboseFormatter(selected.Entity);
        ResultRenderer.RenderResult(entitySelectedFormated);

        return selected.Entity;
    }

    private SelectionPrompt<IEntityFormatter<T>> CreatePrompt(List<T> entities)
    {
        var entitiesFormatted = FormatEntities(entities);

        var prompt = new SelectionPrompt<IEntityFormatter<T>>()
                .Title("[cyan bold]Choose one:[/]")
                .PageSize(3)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .HighlightStyle(new Style(foreground: Color.Aqua))
                .AddChoices(entitiesFormatted);

        return prompt;
    }

    private List<IEntityFormatter<T>> FormatEntities(List<T> entities)
    {
        List<IEntityFormatter<T>> formattedEntities = new();
        foreach (var entity in entities)
        {
            var formatter = _formatterFactory.CreateSimpleFormatter(entity);
            if (formatter is not null)
            {
                formattedEntities.Add(formatter);
            }
        }

        return formattedEntities;
    }
}

namespace LibrarySystem;

public class SimpleFineFormatter : IEntityFormatter<Fine>
{
    public SimpleFineFormatter(Fine entity) : base(entity)
    {
    }

    public override string ToString()
    {
        var statusFineFormatted = _entity.WasPayed ?
            "$ | [bold green] paid [/]" :
            "$ | [bold red] active [/]";

        return $"[bold plum3]Fine:[/]" + statusFineFormatted;
    }
}
namespace LibrarySystem;

public static class ConsoleFormatter
{
    public static string AsAnError(string input)
    {
        return $"[red bold]:warning: {input} [/]";
    }

    public static string AsAnInfo(string input)
    {
        return $"[yellow bold]:magnifying_glass_tilted_left: Info: {input} [/]";
    }

    public static string AsSuccess(string input)
    {
        return $"[green bold]:check_mark: {input}[/]";
    }

    public static string AsIndicator(string input)
    {
        return $"[bold cyan]---- {input} ----[/]";
    }

    public static string AsAHighlight(string input)
    {
        return $"[bold cyan]{input}[/]";
    }
}

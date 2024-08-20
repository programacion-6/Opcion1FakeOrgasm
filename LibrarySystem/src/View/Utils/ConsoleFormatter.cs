namespace LibrarySystem;

public static class ConsoleFormatter
{
    private const string NEW_LINE = "\n";
    private const string SINGLE_SPACE = " ";

    private const string BOLD = "\u001b[1m";
    private const string RED = "\u001b[31m";
    private const string YELLOW = "\u001b[33m";
    private const string GREEN = "\u001b[32m";
    private const string CYAN = "\u001b[36m";
    private const string RESET = "\u001b[0m";

    public static string AsAnError(string input)
    {
        var indicator = "ERROR:";
        var formated = RED + indicator + SINGLE_SPACE + input + NEW_LINE + RESET;
        return formated;
    }

    public static string AsAnInfo(string input)
    {
        var indicator = "INFO:";
        var formated = YELLOW + indicator + SINGLE_SPACE + input + NEW_LINE + RESET;
        return formated;
    }

    public static string AsSuccess(string input)
    {
        var indicator = "✔";
        var formated = GREEN + indicator + SINGLE_SPACE + input + NEW_LINE + RESET;
        return formated;
    }

    public static string AsIndicator(string input)
    {
        var border = "----";
        var formated = CYAN + NEW_LINE + border + SINGLE_SPACE + input + SINGLE_SPACE + border + NEW_LINE + RESET;
        return formated;
    }

    public static string AsAHighlight(string input)
    {
        return CYAN + BOLD + input + RESET;
    }
}

using Spectre.Console;

namespace LibrarySystem;

public class ConsoleReceiver : IReceiver<string>
{
    public string ReceiveInput()
    {
        var indicator = ConsoleFormatter.AsAHighlight("\n>> ");
        AnsiConsole.Markup(indicator);
        var inputReceived = Console.ReadLine();
        if (inputReceived is null)
        {
            var invalidInputError = ConsoleFormatter.AsAnError("invalid input");
            AnsiConsole.MarkupLine(invalidInputError);
            return ReceiveInput();
        }
        else
        {
            return inputReceived;
        }
    }
}
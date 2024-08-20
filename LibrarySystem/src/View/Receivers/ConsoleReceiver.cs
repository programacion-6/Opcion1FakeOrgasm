namespace LibrarySystem;

public class ConsoleReceiver : IReceiver<string>
{
    public string ReceiveInput()
    {
        var indicator = ConsoleFormatter.AsAHighlight(">> ");
        Console.Write(indicator);
        var inputReceived = Console.ReadLine();
        if (inputReceived is null)
        {
            var invalidInputError = ConsoleFormatter.AsAnError("invalid input");
            Console.WriteLine(invalidInputError);
            return ReceiveInput();
        }
        else
        {
            return inputReceived;
        }
    }
}
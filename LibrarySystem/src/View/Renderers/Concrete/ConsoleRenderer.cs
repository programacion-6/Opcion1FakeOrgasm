namespace LibrarySystem;

public class ConsoleRenderer : IViewRenderer<string>
{
    public void Render(string text)
    {
        Console.WriteLine(text);
    }

    public void RenderAsContainer(List<string> texts)
    {
        foreach (var text in texts)
        {
            Console.WriteLine(text);
        }
    }
}
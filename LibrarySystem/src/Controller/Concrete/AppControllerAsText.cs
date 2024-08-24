using Spectre.Console;

namespace LibrarySystem;

public class AppControllerAsText : IAppController
{
    private AbstractViewChanger<string> _viewChanger;
    private IReceiver<string> _receiver;
    private IMessageRenderer _messageRenderer;
    private IExecutableHandler<string>? currentHandler;
    private readonly Dictionary<MenuView, IExecutableHandler<string>> _controllers;

    public AppControllerAsText(AbstractViewChanger<string> viewChanger, IReceiver<string> receiver, Dictionary<MenuView, IExecutableHandler<string>> controllers, IMessageRenderer messageRenderer)
    {
        _viewChanger = viewChanger;
        _controllers = controllers;
        _receiver = receiver;
        _messageRenderer = messageRenderer;
    }

    private async Task Execute(string inputReceived)
    {
        if (_viewChanger.IsTheViewChanging(inputReceived))
        {
            ChangeView(inputReceived);
        }
        else
        {
            if (currentHandler != null)
            {
                await currentHandler.Execute(inputReceived);
            }
        }
    }

    private void SelectInitView()
    {
        while (currentHandler is null)
        {
            var input = _receiver.ReceiveInput();
            if (_viewChanger.IsTheViewChanging(input))
            {
                ChangeView(input);

            }
            if (currentHandler is null)
            {
                _messageRenderer.RenderErrorMessage("view not found, select one...");
            }
        }
    }

    private void ChangeView(string input)
    {
        _viewChanger.ChangeView(input);
        if (_controllers.TryGetValue(_viewChanger.CurrentView, out var executable))
        {
            currentHandler = executable;
            _messageRenderer.RenderSuccessMessage("chosen view");
        }
    }

    public async Task ExecuteInfinitelyAsync()
    {
        ShowWelcome();
        SelectInitView();
        while (true)
        {
            var input = _receiver.ReceiveInput();
            await Execute(input);
        }
    }

    private void ShowWelcome()
    {
        AnsiConsole.Write(
            new FigletText("Welcome")
                .Centered()
                .Color(Color.Plum3));
    }
}
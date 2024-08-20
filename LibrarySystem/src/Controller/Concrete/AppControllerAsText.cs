namespace LibrarySystem;

public class AppControllerAsText : IAppController
{
    private AbstractViewChanger<string> _viewChanger;
    private IReceiver<string> _receiver;
    private AbstractMessageRenderer _messageRenderer;
    private IExecutableHandler<string>? currentHandler;
    private readonly Dictionary<MenuView, IExecutableHandler<string>> _controllers;

    public AppControllerAsText(AbstractViewChanger<string> viewChanger, IReceiver<string> receiver, Dictionary<MenuView, IExecutableHandler<string>> controllers, AbstractMessageRenderer messageRenderer)
    {
        _viewChanger = viewChanger;
        _controllers = controllers;
        _receiver = receiver;
        _messageRenderer = messageRenderer;
    }

    private void Execute(string inputReceived)
    {
        if (_viewChanger.IsTheViewChanging(inputReceived))
        {
            ChangeView(inputReceived);
        }
        else
        {
            currentHandler?.Execute(inputReceived);
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

    public void ExecuteInfinitely()
    {
        ShowWelcome();
        SelectInitView();
        while (true)
        {
            var input = _receiver.ReceiveInput();
            Execute(input);
        }
    }

    private void ShowWelcome()
    {
        _messageRenderer.RenderIndicatorMessage("Welcome");
    }
}
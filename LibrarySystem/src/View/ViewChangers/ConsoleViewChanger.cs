namespace LibrarySystem;

public class ConsoleViewChanger : AbstractViewChanger<string>
{
    private readonly Dictionary<string, MenuView> viewMap = new Dictionary<string, MenuView>
        {
            { "view books", MenuView.BOOK_HANDLER },
            { "view patrons", MenuView.PATRON_HANDLER },
            { "view loans", MenuView.LOAN_MANAGER },
            { "view reports", MenuView.REPORT_MANAGER },
            { "view fines", MenuView.DEBT_MANAGER }
        };

    public override bool IsTheViewChanging(string inputReceived)
    {
        return viewMap.ContainsKey(inputReceived);
    }

    public override void ChangeView(string inputReceived)
    {
        if (viewMap.TryGetValue(inputReceived, out var newView))
        {
            CurrentView = newView;
        }
    }
}
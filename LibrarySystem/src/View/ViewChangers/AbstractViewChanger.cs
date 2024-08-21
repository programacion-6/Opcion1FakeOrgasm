namespace LibrarySystem;

public abstract class AbstractViewChanger<I>
{
    public MenuView CurrentView { get; set; }

    public abstract bool IsTheViewChanging(I inputReceived);

    public abstract void ChangeView(I inputReceived);
}
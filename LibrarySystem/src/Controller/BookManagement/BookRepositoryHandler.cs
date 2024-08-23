namespace LibrarySystem.Controller.BookManagement;

public class BookRepositoryHandler 
{
    private IEntityCreator<Book, string> _bookCreator;
    private IEntityUpdater<Book, string> _bookUpdater;
    private IEntityEliminator<Book, string> _bookEliminator;

    public BookRepositoryHandler(IEntityCreator<Book, string> bookCreator,
        IEntityUpdater<Book, string> bookUpdater, IEntityEliminator<Book, string> bookEliminator)
    {
        _bookCreator = bookCreator;
        _bookUpdater = bookUpdater;
        _bookEliminator = bookEliminator;
    }

    public async Task HandleNewCommand()
    {
        await _bookCreator.TryToCreateEntity();
    }

    public async Task HandleDeleteCommand()
    {
        await _bookEliminator.TryToDeleteEntity();
    }

    public async Task HandleUpdateCommand()
    {
        await _bookUpdater.TryToUpdateEntity();
    }
}

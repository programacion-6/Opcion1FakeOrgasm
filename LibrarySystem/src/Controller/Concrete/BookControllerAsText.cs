namespace LibrarySystem;

public class BookControllerAsText : IExecutableHandler<string>
{
    private IBookRepository _repository;
    private IReceiver<string> _receiver;
    private IEntityCreator<Book, string> _bookCreator;
    private IEntityUpdater<Book, string> _bookUpdater;
    private IEntityEliminator<Book, string> _bookEliminator;
    private IMessageRenderer _messageRenderer;
    private IResultRenderer<Book> _rendererBooks;

    public BookControllerAsText(IBookRepository repository, IEntityCreator<Book, string> bookCreator, IEntityUpdater<Book, string> bookUpdater, IEntityEliminator<Book, string> bookEliminator, IResultRenderer<Book> rendererBooks, IReceiver<string> receiver, IMessageRenderer messageRenderer)
    {
        _repository = repository;
        _bookCreator = bookCreator;
        _bookUpdater = bookUpdater;
        _bookEliminator = bookEliminator;

        _rendererBooks = rendererBooks;
        _receiver = receiver;
        _messageRenderer = messageRenderer;
    }

    public async Task Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "new":
                await _bookCreator.TryToCreateEntity();
                break;
            case "delete":
                await _bookEliminator.TryToDeleteEntity();
                break;
            case "update":
                await _bookUpdater.TryToUpdateEntity();
                break;
            case "show all":
                var allBooks = await _repository.GetAll();
                _rendererBooks.RenderResults(allBooks.ToList());
                break;
            case "show by genre":
                await FindBooksByGenre();
                break;
            case "find by title":
                await FindBookByTitle();
                break;
            case "find by author":
                await FindBookByAuthor();
                break;
            case "find by ISBN":
                await FindBookByISBN();
                break;
            default:
                _messageRenderer.RenderErrorMessage("option not found");
                break;
        }
    }

    private async Task FindBooksByGenre()
    {
        _messageRenderer.RenderSimpleMessage("Enter the genre:");
        var genre = _receiver.ReceiveInput();
        var booksFound = await _repository.GetBooksByGenre(genre);
        _rendererBooks.RenderResults(booksFound.ToList());
    }

    private async Task FindBookByTitle()
    {
        _messageRenderer.RenderSimpleMessage("Enter the title:");
        var title = _receiver.ReceiveInput();
        var bookFound = await _repository.GetByTitle(title);
        _rendererBooks.RenderResult(bookFound);
    }

    private async Task FindBookByAuthor()
    {
        _messageRenderer.RenderSimpleMessage("Enter the author:");
        var author = _receiver.ReceiveInput();
        var bookFound = await _repository.GetByAuthor(author);
        _rendererBooks.RenderResult(bookFound);
    }

    private async Task FindBookByISBN()
    {
        _messageRenderer.RenderSimpleMessage("Enter the ISBN:");
        var ISBN = _receiver.ReceiveInput();
        var bookFound = await _repository.GetByISBN(ISBN);
        _rendererBooks.RenderResult(bookFound);
    }

}
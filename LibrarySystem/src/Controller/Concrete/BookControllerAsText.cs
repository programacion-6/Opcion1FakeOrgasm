namespace LibrarySystem;

public class BookControllerAsText : IExecutableHandler<string>
{
    private IBookRepository _repository;
    private IReceiver<string> _receiver;
    private IEntityCreator<Book, string> _bookCreator;
    private IEntityUpdater<Book, string> _bookUpdater;
    private IEntityEliminator<Book, string> _bookEliminator;
    private IMessageRenderer _messageRenderer;
    private IEntityFormatterFactory<Book> _bookFormatterFactory;

    public BookControllerAsText(IBookRepository repository, IEntityCreator<Book, string> bookCreator, IEntityUpdater<Book, string> bookUpdater, IEntityEliminator<Book, string> bookEliminator, IEntityFormatterFactory<Book> bookFormatterFactory, IReceiver<string> receiver, IMessageRenderer messageRenderer)
    {
        _repository = repository;
        _bookCreator = bookCreator;
        _bookUpdater = bookUpdater;
        _bookEliminator = bookEliminator;

        _bookFormatterFactory = bookFormatterFactory;
        _receiver = receiver;
        _messageRenderer = messageRenderer;
    }

    public void Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "new":
                _bookCreator.TryToCreateEntity();
                break;
            case "delete":
                _bookEliminator.TryToDeleteEntity();
                break;
            case "update":
                _bookUpdater.TryToUpdateEntity();
                break;
            case "show all":
                ShowAll();
                break;
            case "show by genre":
                FindBooksByGenre();
                break;
            case "find by title":
                FindBookByTitle();
                break;
            case "find by author":
                FindBookByAuthor();
                break;
            case "find by ISBN":
                FindBookByISBN();
                break;
            default:
                _messageRenderer.RenderErrorMessage("option not found");
                break;
        }
    }

    private void ShowAll()
    {
        var allBooks = _repository.GetAll();
        RenderBooksFound(allBooks);
    }

    private void FindBooksByGenre()
    {
        _messageRenderer.RenderSimpleMessage("Enter the genre:");
        var genre = _receiver.ReceiveInput();
        var booksFound = _repository.GetBooksByGenre(genre);
        RenderBooksFound(booksFound);
    }

    private void FindBookByTitle()
    {
        _messageRenderer.RenderSimpleMessage("Enter the title:");
        var title = _receiver.ReceiveInput();
        var bookFound = _repository.GetByTitle(title);
        RenderBookFound(bookFound);
    }

    private void FindBookByAuthor()
    {
        _messageRenderer.RenderSimpleMessage("Enter the author:");
        var author = _receiver.ReceiveInput();
        var bookFound = _repository.GetByAuthor(author);
        RenderBookFound(bookFound);
    }

    private void FindBookByISBN()
    {
        _messageRenderer.RenderSimpleMessage("Enter the ISBN:");
        var ISBN = _receiver.ReceiveInput();
        var bookFound = _repository.GetByISBN(ISBN);
        RenderBookFound(bookFound);
    }

    private void RenderBookFound(Book? bookFound)
    {
        if (bookFound is not null)
        {
            var bookFormated = _bookFormatterFactory.CreateFormatter(bookFound, FormatType.Detailed);
            ResultRenderer.RenderResult(bookFormated);
        }
        else
        {
            ResultRenderer.RenderResult(bookFound);
        }
    }

    private void RenderBooksFound(List<Book> booksFound)
    {
        var booksFormated = booksFound.Select
            (book => _bookFormatterFactory
                    .CreateFormatter(book, FormatType.Simple))
                    .ToList();
        ResultRenderer.RenderResults(booksFormated);
    }

}
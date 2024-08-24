using Spectre.Console;

namespace LibrarySystem;

public class BookControllerAsText : IExecutableHandler<string>
{
    private IBookRepository _repository;
    private IEntityCreator<Book, string> _bookCreator;
    private IEntityUpdater<Book, string> _bookUpdater;
    private IEntityEliminator<Book, string> _bookEliminator;
    private IMessageRenderer _messageRenderer;
    private IEntityFormatterFactory<Book> _bookFormatterFactory;
    private EntityRendererAsConsolePages<Book> _bookRendererAsPages;

    public BookControllerAsText(IBookRepository repository, IEntityCreator<Book, string> bookCreator, IEntityUpdater<Book, string> bookUpdater, IEntityEliminator<Book, string> bookEliminator, IEntityFormatterFactory<Book> bookFormatterFactory, IMessageRenderer messageRenderer, EntityRendererAsConsolePages<Book> bookRendererAsPages)
    {
        _repository = repository;
        _bookCreator = bookCreator;
        _bookUpdater = bookUpdater;
        _bookEliminator = bookEliminator;

        _bookFormatterFactory = bookFormatterFactory;
        _messageRenderer = messageRenderer;
        _bookRendererAsPages = bookRendererAsPages;
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
                await _bookRendererAsPages.RenderByPagination();
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
        var genre = AnsiConsole.Ask<string>("Enter the [bold]genre[/]:");
        var booksFound = await _repository.GetBooksByGenre(genre);
        await RenderVerboseBooksFormatted(booksFound.ToList());
    }

    private async Task FindBookByTitle()
    {
        var title = AnsiConsole.Ask<string>("Enter the [bold]title[/]:");
        var bookFound = await _repository.GetByTitle(title);
        var bookFormated = await _bookFormatterFactory.CreateVerboseFormatter(bookFound);
        ResultRenderer.RenderResult(bookFormated);
    }

    private async Task FindBookByAuthor()
    {
        var author = AnsiConsole.Ask<string>("Enter the [bold]author[/]:");
        var bookFound = await _repository.GetByAuthor(author);
        var bookFormated = await _bookFormatterFactory.CreateVerboseFormatter(bookFound);
        ResultRenderer.RenderResult(bookFormated);
    }

    private async Task FindBookByISBN()
    {
        var ISBN = AnsiConsole.Ask<string>("Enter the [bold]ISBN[/]:");
        var bookFound = await _repository.GetByISBN(ISBN);
        var bookFormated = await _bookFormatterFactory.CreateVerboseFormatter(bookFound);
        ResultRenderer.RenderResult(bookFormated);
    }

    private async Task RenderVerboseBooksFormatted(List<Book> books)
    {
        var booksFormated = await Task.WhenAll(books.Select(async book =>
                                    await _bookFormatterFactory
                                    .CreateVerboseFormatter(book)));

        ResultRenderer.RenderResults(booksFormated.ToList());
    }

}
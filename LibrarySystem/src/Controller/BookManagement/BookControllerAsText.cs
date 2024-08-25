using Spectre.Console;

namespace LibrarySystem.Controller.BookManagement;

public class BookControllerAsText : IExecutableHandler<string>
{
    private IBookRepository _repository;
    private IMessageRenderer _messageRenderer;
    private IEntityFormatterFactory<Book> _bookFormatterFactory;
    private EntityRendererAsConsolePages<Book> _bookRendererAsPages;

    private BookSearcher _searcher;
    private BookRepositoryHandler _repositoryHandler;

    public BookControllerAsText(
        IMessageRenderer messageRenderer,
        BookSearcher searcher,
        BookRepositoryHandler repositoryHandler,
        IBookRepository repository,
        IEntityFormatterFactory<Book> bookFormatterFactory,
        EntityRendererAsConsolePages<Book> bookRendererAsPages
    )
    {
        _searcher = searcher;
        _repositoryHandler = repositoryHandler;
        _repository = repository;
        _bookFormatterFactory = bookFormatterFactory;
        _messageRenderer = messageRenderer;
        _bookRendererAsPages = bookRendererAsPages;
    }

    public async Task Execute(string inputReceived)
    {
        switch (inputReceived)
        {
            case "new":
                await _repositoryHandler.HandleNewCommand();
                break;
            case "delete":
                await _repositoryHandler.HandleDeleteCommand();
                break;
            case "update":
                await _repositoryHandler.HandleUpdateCommand();
                break;
            case "show all":
                await _bookRendererAsPages.RenderByPagination();
                break;
            case "show by genre":
                await FindBooksByGenre();
                break;
            case "find by title":
                await _searcher.FindBookByTitle();
                break;
            case "find by author":
                await _searcher.FindBookByAuthor();
                break;
            case "find by ISBN":
                await _searcher.FindBookByISBN();
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


    private async Task RenderVerboseBooksFormatted(List<Book> books)
    {
        var booksFormated = await Task.WhenAll(books.Select(async book =>
                                    await _bookFormatterFactory
                                    .CreateVerboseFormatter(book)));

        ResultRenderer.RenderResults(booksFormated.ToList());
    }
}

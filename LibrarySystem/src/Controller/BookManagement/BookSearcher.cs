using Spectre.Console;
namespace LibrarySystem.Controller.BookManagement;

public class BookSearcher
{
    private IBookRepository _repository;
    private IEntityFormatterFactory<Book> _bookFormatterFactory;

    public BookSearcher(IBookRepository repository, IEntityFormatterFactory<Book> bookFormatterFactory)
    {
        _repository = repository;
        _bookFormatterFactory = bookFormatterFactory;
    }

    public async Task FindBookByTitle()
    {
        var title = AnsiConsole.Ask<string>("Enter the [bold]title[/]:");
        var bookFound = await _repository.GetByTitle(title);
        var bookFormated = await _bookFormatterFactory.CreateVerboseFormatter(bookFound);
        ResultRenderer.RenderResult(bookFormated);
    }

    public async Task FindBookByAuthor()
    {
        var author = AnsiConsole.Ask<string>("Enter the [bold]author[/]:");
        var bookFound = await _repository.GetByAuthor(author);
        var bookFormated = await _bookFormatterFactory.CreateVerboseFormatter(bookFound);
        ResultRenderer.RenderResult(bookFormated);
    }

    public async Task FindBookByISBN()
    {
        var ISBN = AnsiConsole.Ask<string>("Enter the [bold]ISBN[/]:");
        var bookFound = await _repository.GetByISBN(ISBN);
        var bookFormated = await _bookFormatterFactory.CreateVerboseFormatter(bookFound);
        ResultRenderer.RenderResult(bookFormated);
    }
}


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

    public BookControllerAsText(IBookRepository repository, IEntityCreator<Book, string> bookCreator, IEntityUpdater<Book, string> bookUpdater, IEntityEliminator<Book, string> bookEliminator, IEntityFormatterFactory<Book> bookFormatterFactory, IMessageRenderer messageRenderer)
    {
        _repository = repository;
        _bookCreator = bookCreator;
        _bookUpdater = bookUpdater;
        _bookEliminator = bookEliminator;

        _bookFormatterFactory = bookFormatterFactory;
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
                await ShowAll();
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

    private async Task ShowAll()
    {
        await PaginateBooks(async () => (await _repository.GetAll()).ToList());
    }

    private async Task FindBooksByGenre()
    {
        var genre = AnsiConsole.Ask<string>("Enter the [bold]genre[/]:");
        await PaginateBooks(async () => (await _repository.GetBooksByGenre(genre)).ToList());
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
        var bookFormatted = await _bookFormatterFactory.CreateVerboseFormatter(bookFound);
        ResultRenderer.RenderResult(bookFormatted);
    }

    private async Task PaginateBooks(Func<Task<List<Book>>> getBooks)
    {
        var allBooks = await getBooks();
        int pageSize = 3;  // Define el tamaño de página
        int currentPage = 0;
        bool exit = false;

        while (!exit)
        {
            // Guardar la posición actual del cursor
            var top = Console.CursorTop;

            // Obtener los libros para la página actual
            var pageBooks = allBooks.Skip(currentPage * pageSize).Take(pageSize).ToList();

            // Imprimir la página actual
            AnsiConsole.Write(new Markup($"[bold underline]Page {currentPage + 1}[/]\n\n"));
            await RenderVerboseBooksFormatted(pageBooks);

            // Mostrar opciones de navegación
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(new[] { "Next", "Previous", "Exit" }));

            switch (choice)
            {
                case "Next":
                    if ((currentPage + 1) * pageSize < allBooks.Count)
                    {
                        AnsiConsole.Clear();
                        currentPage++;
                        
                    }
                    else
                    {
                        AnsiConsole.Clear();
                    }
                    break;
                case "Previous":
                    if (currentPage > 0)
                    {
                        AnsiConsole.Clear();
                        currentPage--;
                    }
                    else
                    {
                        AnsiConsole.Clear();
                    }
                    break;
                case "Exit":
                    exit = true;
                    break;
            }
        }
    }

    private async Task RenderVerboseBooksFormatted(List<Book> books)
    {
        var booksFormatted = await Task.WhenAll(books.Select(async book =>
            await _bookFormatterFactory.CreateVerboseFormatter(book)));

        ResultRenderer.RenderResults(booksFormatted.ToList());
    }
}

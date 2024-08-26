using Spectre.Console;

namespace LibrarySystem;

public class BookRequesterByConsole : IEntityRequester<Book>
{
    private IMessageRenderer _renderer;
    private BookValidator _bookValidator;

    private ErrorLogger _errorLogger;

    public BookRequesterByConsole(IMessageRenderer renderer)
    {
        _renderer = renderer;
        _bookValidator = new BookValidator();
        _errorLogger = new ErrorLogger();
    }

    public Book? AskForEntity()
    {
        Book? requestedBook = null;
        try
        {
            var bookToValidate = ReceiveBookByConsole();
            _bookValidator.ValidateBook(bookToValidate);
            requestedBook = bookToValidate;
        }
        catch (BookException ex)
        {
            _errorLogger.LogErrorBasedOnSeverity(ex.Severity, ex.Message, ex);
            _renderer.RenderErrorMessage($"{ex.Message} \n...{ex.ResolutionSuggestion}");
        }
        catch (Exception ex)
        {
            _errorLogger.LogErrorBasedOnSeverity(SeverityLevel.High, "", ex);
            _renderer.RenderErrorMessage(ex.Message);
        }

        return requestedBook;
    }

    private Book ReceiveBookByConsole()
    {
        var title = AnsiConsole.Ask<string>("Enter the [bold]title[/]:");
        var author = AnsiConsole.Ask<string>("Enter the [bold]author[/]:");
        var isbn = AnsiConsole.Ask<string>("Enter the [bold]ISBN[/]:");
        var genre = AnsiConsole.Ask<string>("Enter the [bold]genre[/]:");
        var year = AnsiConsole.Ask<int>("Enter the [bold]publication year[/]:");

        var bookReceived = new Book
        {
            Id = Guid.NewGuid(),
            Title = title,
            Author = author,
            ISBN = isbn,
            Genre = genre,
            PublicationYear = year
        };

        return bookReceived;
    }

}

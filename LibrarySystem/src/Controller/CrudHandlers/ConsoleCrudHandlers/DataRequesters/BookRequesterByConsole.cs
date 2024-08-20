namespace LibrarySystem;

public class BookRequesterByConsole : IEntityRequester<Book>
{
    private AbstractMessageRenderer _renderer;
    private IReceiver<string> _receiver;

    public BookRequesterByConsole(AbstractMessageRenderer renderer, IReceiver<string> receiver)
    {
        _renderer = renderer;
        _receiver = receiver;
    }

    public Book? AskForEntity()
    {
        Book? requestedBook = null;
        try
        {
            var bookToValidate = ReceiveBookByConsole();
            BookValidator.ValidateBook(bookToValidate);
            requestedBook = bookToValidate;
        }
        catch (BookException ex)
        {
            _renderer.RenderErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            _renderer.RenderErrorMessage(ex.Message);
        }

        return requestedBook;
    }

    private Book ReceiveBookByConsole()
    {
        _renderer.RenderSimpleMessage("Enter the title: ");
        var title = _receiver.ReceiveInput();
        _renderer.RenderSimpleMessage("Enter the author: ");
        var author = _receiver.ReceiveInput();
        _renderer.RenderSimpleMessage("Enter the ISBN: ");
        var isbn = _receiver.ReceiveInput();
        _renderer.RenderSimpleMessage("Enter the genre: ");
        var genre = _receiver.ReceiveInput();
        var year = ReceiveYearAsNumber();

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

    private int ReceiveYearAsNumber()
    {
        int yearAsNumber = -1;
        _renderer.RenderSimpleMessage("Enter the publication year: ");
        do
        {
            var yearAsText = _receiver.ReceiveInput();
            if (int.TryParse(yearAsText, out int number))
            {
                yearAsNumber = number;
            }
            else
            {
                _renderer.RenderErrorMessage("enter a number");

            }
        } while (yearAsNumber == -1);

        return yearAsNumber;
    }
}

namespace LibrarySystem;

public class VerboseLoanFormatter : IEntityVerboseFormatter<Loan>
{
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;
    private Book? _book;
    private Patron? _patron;

    public VerboseLoanFormatter(Loan entity, IBookRepository bookRepository, IPatronRepository patronRepository) : base(entity)
    {
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public override async Task LoadRelatedData()
    {
        if (Entity is not null)
        {
            _book = await _bookRepository.GetById(_entity.BookId);
            _patron = await _patronRepository.GetById(_entity.PatronId);
        }
    }

    private string GetBookFormatted()
    {
        var bookFormatted = "[lightsteelblue1] no loaded [/]";

        if (_book is not null)
        {
            bookFormatted = new SimpleBookFormatter(_book).ToString();
        }

        return bookFormatted;
    }

    private string GetPatronFormatted()
    {
        var patronFormatted = "[lightsteelblue1] no loaded [/]";

        if (_patron is not null)
        {
            patronFormatted = new SimplePatronDetailsFormatter(_patron).ToString();
        }

        return patronFormatted;
    }

    public override string ToString()
    {
        var statusFineFormatted = _entity.WasReturn ?
            " | [bold green] returned [/]" :
            " | [bold red] active [/]";

        return "[bold plum3]Loan:[/]" + statusFineFormatted
                    + "\n    " + $"[grey74]{_entity.LoanDate} - {_entity.ReturnDate}[/]"
        + "\n    [bold]Book: [/]" + GetBookFormatted()
             + "\n    [bold]Patron: [/]" + GetPatronFormatted();
    }
}
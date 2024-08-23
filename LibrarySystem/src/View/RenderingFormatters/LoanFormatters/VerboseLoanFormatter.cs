namespace LibrarySystem;

public class VerboseLoanFormatter : IEntityVerboseFormatter<Loan>
{
    private readonly Loan _entity;
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;
    private Book? _book;
    private Patron? _patron;

    public VerboseLoanFormatter(Loan entity, IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _entity = entity;
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public async Task LoadRelatedData()
    {
        if (Entity is not null)
        {
            _book = await _bookRepository.GetById(_entity.BookId);
            _patron = await _patronRepository.GetById(_entity.PatronId);
        }
    }

    public override string ToString()
    {
        return "Loan " + (_entity.WasReturn ? "returned" : "active") + " | "
            + _entity.LoanDate + " - " + _entity.ReturnDate
            + "\n\tBook: " + GetBookFormatted()
             + "\n\tPatron: " + GetPatronFormatted();
    }

    private string GetBookFormatted()
    {
        var bookFormatted = "no loaded";

        if (_book is not null)
        {
            bookFormatted = new SimpleBookFormatter(_book).ToString();
        }

        return bookFormatted;
    }

    private string GetPatronFormatted()
    {
        var patronFormatted = "no loaded";

        if (_patron is not null)
        {
            patronFormatted = new SimplePatronDetailsFormatter(_patron).ToString();
        }

        return patronFormatted;
    }

    public Loan Entity
    {
        get => _entity;
    }
}
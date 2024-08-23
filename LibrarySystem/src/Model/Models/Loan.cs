namespace LibrarySystem;

public class Loan : EntityBase
{
    private Guid _bookId;
    private Guid _patronId;
    private DateTime _loanDate = DateTime.Now;
    private DateTime _returnDate;
    private bool _wasReturn = false;

    public required Guid BookId
    {
        get => _bookId;
        set => _bookId = value;
    }

    public required  Guid PatronId
    {
        get => _patronId;
        set => _patronId = value;
    }

    public DateTime LoanDate
    {
        get => _loanDate;
        set => _loanDate = value;
    }

    public required DateTime ReturnDate
    {
        get => _returnDate;
        set => _returnDate = value;
    }

    public bool WasReturn
    {
        get => _wasReturn;
        set => _wasReturn = value;
    }
}
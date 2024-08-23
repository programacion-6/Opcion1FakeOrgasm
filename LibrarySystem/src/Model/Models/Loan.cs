namespace LibrarySystem;

public class Loan : EntityBase
{
    private Guid _idBook;
    private Guid _idPatron;
    private DateTime _loanDate = DateTime.Now;
    private DateTime _returnDate;
    private bool _wasReturn = false;

    public Guid IdBook
    {
        get => _idBook;
        set => _idBook = value;
    }

    public Guid IdPatron
    {
        get => _idPatron;
        set => _idPatron = value;
    }

    public DateTime LoanDate
    {
        get => _loanDate;
        set => _loanDate = value;
    }

    public DateTime ReturnDate
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
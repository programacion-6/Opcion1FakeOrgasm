namespace LibrarySystem;

public class LoanException : Exception
{
    public LoanException(string message)
            : base(message)
    {
    }
}

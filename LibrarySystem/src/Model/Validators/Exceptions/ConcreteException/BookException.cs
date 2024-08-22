namespace LibrarySystem;

public class BookException : CustomException
{
    public BookException(string message, SeverityLevel severity = SeverityLevel.Medium, string resolutionSuggestion = "")
            : base(message, severity, resolutionSuggestion)
    {
    }
}

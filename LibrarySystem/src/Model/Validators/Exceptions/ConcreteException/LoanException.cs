namespace LibrarySystem;

public class LoanException : CustomException
{
    public LoanException(string message, SeverityLevel severity = SeverityLevel.Medium, string resolutionSuggestion = "")
            : base(message, severity, resolutionSuggestion)
    {
    }
}

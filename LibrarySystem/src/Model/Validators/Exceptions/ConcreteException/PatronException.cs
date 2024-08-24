namespace LibrarySystem;

public class PatronException : CustomException
{
    public PatronException(string message, SeverityLevel severity = SeverityLevel.Medium, string resolutionSuggestion = "")
            : base(message, severity, resolutionSuggestion)
    {
    }
}

namespace LibrarySystem;

public class CustomException : Exception
{
    public SeverityLevel Severity { get; }
    public int ErrorCode { get; }
    public string ResolutionSuggestion { get; }

    public CustomException(string message, SeverityLevel severityLevel, int errorCode = 0, string resolutionSuggestion = "")
        : base(message)
    {
        Severity = severityLevel;
        ErrorCode = errorCode;
        ResolutionSuggestion = resolutionSuggestion;
    }
}
using Serilog;

namespace LibrarySystem;

public class ErrorLogger
{
    public void LogErrorBasedOnSeverity(SeverityLevel severity, string message, Exception ex)
    {
        switch (severity)
        {
            case SeverityLevel.Critical:
                Log.Fatal(ex, message);
                break;
            case SeverityLevel.High:
                Log.Error(ex, message);
                break;
            case SeverityLevel.Medium:
                Log.Warning(ex, message);
                break;
            case SeverityLevel.Low:
                Log.Information(ex, message);
                break;
            default:
                Log.Error(ex, message);
                break;
        }
    }
}
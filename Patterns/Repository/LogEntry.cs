namespace Patterns.Repository;

/// <summary>
/// Класс, представляющий запись лога
/// </summary>
public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Source { get; set; }

    public LogEntry()
    {
        Timestamp = DateTime.Now;
    }

    public LogEntry(string level, string message, string? source = null)
    {
        Timestamp = DateTime.Now;
        Level = level;
        Message = message;
        Source = source;
    }
}


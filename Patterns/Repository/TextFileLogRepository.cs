using System.Text;

namespace Patterns.Repository;

/// <summary>
/// Репозиторий для записи логов в текстовый файл построчно
/// </summary>
public class TextFileLogRepository : ILogRepository
{
    private readonly string _filePath;
    private readonly object _lockObject = new();

    public TextFileLogRepository(string filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        
        // Создаем директорию, если она не существует
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public async Task WriteLogAsync(LogEntry logEntry)
    {
        if (logEntry == null)
            throw new ArgumentNullException(nameof(logEntry));

        var logLine = $"[{logEntry.Timestamp:yyyy-MM-dd HH:mm:ss}] [{logEntry.Level}] {logEntry.Message}";
        
        if (!string.IsNullOrEmpty(logEntry.Source))
        {
            logLine += $" [Source: {logEntry.Source}]";
        }

        lock (_lockObject)
        {
            File.AppendAllText(_filePath, logLine + Environment.NewLine, Encoding.UTF8);
        }

        await Task.CompletedTask;
    }

    public async Task<IEnumerable<LogEntry>> ReadLogsAsync()
    {
        if (!File.Exists(_filePath))
            return Enumerable.Empty<LogEntry>();

        var logs = new List<LogEntry>();
        
        lock (_lockObject)
        {
            var lines = File.ReadAllLines(_filePath, Encoding.UTF8);
            foreach (var line in lines)
            {
                // Простой парсинг строки лога
                // Формат: [Timestamp] [Level] Message [Source: ...]
                try
                {
                    var entry = ParseLogLine(line);
                    if (entry != null)
                        logs.Add(entry);
                }
                catch
                {
                    // Игнорируем некорректные строки
                }
            }
        }

        return await Task.FromResult(logs);
    }

    private LogEntry? ParseLogLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return null;

        // Упрощенный парсинг - в реальном приложении можно использовать регулярные выражения
        var parts = line.Split(']', 3);
        if (parts.Length < 3)
            return null;

        var timestampStr = parts[0].TrimStart('[');
        var levelStr = parts[1].TrimStart('[').Trim();
        var messageAndSource = parts[2].Trim();

        if (DateTime.TryParse(timestampStr, out var timestamp))
        {
            var entry = new LogEntry
            {
                Timestamp = timestamp,
                Level = levelStr,
                Message = messageAndSource
            };

            // Извлекаем Source, если есть
            var sourceIndex = messageAndSource.IndexOf("[Source:", StringComparison.Ordinal);
            if (sourceIndex >= 0)
            {
                entry.Message = messageAndSource.Substring(0, sourceIndex).Trim();
                var sourcePart = messageAndSource.Substring(sourceIndex + 8).TrimEnd(']').Trim();
                entry.Source = sourcePart;
            }

            return entry;
        }

        return null;
    }
}


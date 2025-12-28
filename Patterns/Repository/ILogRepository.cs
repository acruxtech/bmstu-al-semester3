namespace Patterns.Repository;

/// <summary>
/// Интерфейс репозитория для записи логов
/// </summary>
public interface ILogRepository
{
    /// <summary>
    /// Записывает лог-запись
    /// </summary>
    Task WriteLogAsync(LogEntry logEntry);
    
    /// <summary>
    /// Читает все логи
    /// </summary>
    Task<IEnumerable<LogEntry>> ReadLogsAsync();
}


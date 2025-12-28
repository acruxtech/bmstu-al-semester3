namespace Patterns.Repository;

/// <summary>
/// Класс для логирования с использованием паттерна Repository
/// </summary>
public class MyLogger
{
    private readonly ILogRepository _repository;

    public MyLogger(ILogRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Записывает информационное сообщение
    /// </summary>
    public async Task LogInfoAsync(string message, string? source = null)
    {
        var logEntry = new LogEntry("INFO", message, source);
        await _repository.WriteLogAsync(logEntry);
    }

    /// <summary>
    /// Записывает предупреждение
    /// </summary>
    public async Task LogWarningAsync(string message, string? source = null)
    {
        var logEntry = new LogEntry("WARNING", message, source);
        await _repository.WriteLogAsync(logEntry);
    }

    /// <summary>
    /// Записывает ошибку
    /// </summary>
    public async Task LogErrorAsync(string message, string? source = null)
    {
        var logEntry = new LogEntry("ERROR", message, source);
        await _repository.WriteLogAsync(logEntry);
    }

    /// <summary>
    /// Записывает отладочное сообщение
    /// </summary>
    public async Task LogDebugAsync(string message, string? source = null)
    {
        var logEntry = new LogEntry("DEBUG", message, source);
        await _repository.WriteLogAsync(logEntry);
    }

    /// <summary>
    /// Читает все логи из репозитория
    /// </summary>
    public async Task<IEnumerable<LogEntry>> ReadLogsAsync()
    {
        return await _repository.ReadLogsAsync();
    }
}


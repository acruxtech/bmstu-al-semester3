using Newtonsoft.Json;
using System.Text;

namespace Patterns.Repository;

/// <summary>
/// Репозиторий для записи логов в JSON файл с сохранением разметки JSON
/// </summary>
public class JsonFileLogRepository : ILogRepository
{
    private readonly string _filePath;
    private readonly object _lockObject = new();

    public JsonFileLogRepository(string filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        
        // Создаем директорию, если она не существует
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Инициализируем файл пустым массивом, если файл не существует
        if (!File.Exists(_filePath))
        {
            InitializeJsonFile();
        }
    }

    private void InitializeJsonFile()
    {
        lock (_lockObject)
        {
            File.WriteAllText(_filePath, "[]", Encoding.UTF8);
        }
    }

    public async Task WriteLogAsync(LogEntry logEntry)
    {
        if (logEntry == null)
            throw new ArgumentNullException(nameof(logEntry));

        lock (_lockObject)
        {
            List<LogEntry> logs;
            
            // Читаем существующие логи
            if (File.Exists(_filePath))
            {
                try
                {
                    var json = File.ReadAllText(_filePath, Encoding.UTF8);
                    logs = JsonConvert.DeserializeObject<List<LogEntry>>(json) ?? new List<LogEntry>();
                }
                catch
                {
                    logs = new List<LogEntry>();
                }
            }
            else
            {
                logs = new List<LogEntry>();
            }

            // Добавляем новую запись
            logs.Add(logEntry);

            // Записываем обратно с форматированием JSON
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            var updatedJson = JsonConvert.SerializeObject(logs, settings);
            File.WriteAllText(_filePath, updatedJson, Encoding.UTF8);
        }

        await Task.CompletedTask;
    }

    public async Task<IEnumerable<LogEntry>> ReadLogsAsync()
    {
        if (!File.Exists(_filePath))
            return Enumerable.Empty<LogEntry>();

        List<LogEntry> logs;

        lock (_lockObject)
        {
            try
            {
                var json = File.ReadAllText(_filePath, Encoding.UTF8);
                logs = JsonConvert.DeserializeObject<List<LogEntry>>(json) ?? new List<LogEntry>();
            }
            catch
            {
                logs = new List<LogEntry>();
            }
        }

        return await Task.FromResult(logs);
    }
}


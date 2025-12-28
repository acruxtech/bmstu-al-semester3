using System.Collections.Concurrent;

namespace Patterns.Observer;

/// <summary>
/// Класс для отслеживания изменений в директории с использованием паттерна Observer
/// </summary>
public class FileSystemWatcher : IDisposable
{
    private readonly string _directoryPath;
    private readonly int _checkIntervalMs;
    private readonly List<IFileSystemObserver> _observers = new();
    private readonly object _observersLock = new();
    private Timer? _timer;
    private ConcurrentDictionary<string, FileInfo> _previousFiles = new();
    private bool _disposed = false;

    public FileSystemWatcher(string directoryPath, int checkIntervalMs = 1000)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
            throw new ArgumentException("Directory path cannot be null or empty", nameof(directoryPath));
        
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

        _directoryPath = directoryPath;
        _checkIntervalMs = checkIntervalMs;
        
        // Инициализация начального состояния
        InitializeFileState();
    }

    /// <summary>
    /// Инициализирует начальное состояние файлов в директории
    /// </summary>
    private void InitializeFileState()
    {
        try
        {
            var files = Directory.GetFiles(_directoryPath);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                _previousFiles[file] = fileInfo;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing file state: {ex.Message}");
        }
    }

    /// <summary>
    /// Подписывает наблюдателя на события изменений файловой системы
    /// </summary>
    public void Subscribe(IFileSystemObserver observer)
    {
        if (observer == null)
            throw new ArgumentNullException(nameof(observer));

        lock (_observersLock)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }
    }

    /// <summary>
    /// Отписывает наблюдателя от событий изменений файловой системы
    /// </summary>
    public void Unsubscribe(IFileSystemObserver observer)
    {
        if (observer == null)
            throw new ArgumentNullException(nameof(observer));

        lock (_observersLock)
        {
            _observers.Remove(observer);
        }
    }

    /// <summary>
    /// Уведомляет всех наблюдателей о создании файла
    /// </summary>
    private void NotifyFileCreated(string filePath)
    {
        lock (_observersLock)
        {
            foreach (var observer in _observers)
            {
                try
                {
                    observer.OnFileCreated(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error notifying observer about file creation: {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Уведомляет всех наблюдателей об изменении файла
    /// </summary>
    private void NotifyFileChanged(string filePath)
    {
        lock (_observersLock)
        {
            foreach (var observer in _observers)
            {
                try
                {
                    observer.OnFileChanged(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error notifying observer about file change: {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Уведомляет всех наблюдателей об удалении файла
    /// </summary>
    private void NotifyFileDeleted(string filePath)
    {
        lock (_observersLock)
        {
            foreach (var observer in _observers)
            {
                try
                {
                    observer.OnFileDeleted(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error notifying observer about file deletion: {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Запускает мониторинг директории
    /// </summary>
    public void Start()
    {
        if (_timer != null)
            return;

        _timer = new Timer(CheckDirectory, null, 0, _checkIntervalMs);
        Console.WriteLine($"FileSystemWatcher started monitoring: {_directoryPath}");
    }

    /// <summary>
    /// Останавливает мониторинг директории
    /// </summary>
    public void Stop()
    {
        _timer?.Dispose();
        _timer = null;
        Console.WriteLine("FileSystemWatcher stopped");
    }

    /// <summary>
    /// Проверяет директорию на изменения
    /// </summary>
    private void CheckDirectory(object? state)
    {
        if (_disposed)
            return;

        try
        {
            if (!Directory.Exists(_directoryPath))
            {
                Console.WriteLine($"Directory no longer exists: {_directoryPath}");
                Stop();
                return;
            }

            var currentFiles = new ConcurrentDictionary<string, FileInfo>();
            var files = Directory.GetFiles(_directoryPath);

            // Проверяем текущие файлы
            foreach (var file in files)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    currentFiles[file] = fileInfo;

                    if (_previousFiles.TryGetValue(file, out var previousInfo))
                    {
                        // Файл существует - проверяем изменения
                        if (fileInfo.LastWriteTime != previousInfo.LastWriteTime ||
                            fileInfo.Length != previousInfo.Length)
                        {
                            NotifyFileChanged(file);
                        }
                    }
                    else
                    {
                        // Новый файл
                        NotifyFileCreated(file);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking file {file}: {ex.Message}");
                }
            }

            // Проверяем удаленные файлы
            foreach (var previousFile in _previousFiles.Keys)
            {
                if (!currentFiles.ContainsKey(previousFile))
                {
                    NotifyFileDeleted(previousFile);
                }
            }

            _previousFiles = currentFiles;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking directory: {ex.Message}");
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        Stop();
        _disposed = true;
    }
}


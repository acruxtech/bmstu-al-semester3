namespace Patterns.Observer;

/// <summary>
/// Интерфейс наблюдателя для отслеживания изменений файловой системы
/// </summary>
public interface IFileSystemObserver
{
    /// <summary>
    /// Вызывается при создании файла
    /// </summary>
    void OnFileCreated(string filePath);
    
    /// <summary>
    /// Вызывается при изменении файла
    /// </summary>
    void OnFileChanged(string filePath);
    
    /// <summary>
    /// Вызывается при удалении файла
    /// </summary>
    void OnFileDeleted(string filePath);
}


namespace Patterns.Observer;

/// <summary>
/// Реализация наблюдателя, выводящего события в консоль
/// </summary>
public class ConsoleFileSystemObserver : IFileSystemObserver
{
    public void OnFileCreated(string filePath)
    {
        Console.WriteLine($"[CREATED] {filePath}");
    }

    public void OnFileChanged(string filePath)
    {
        Console.WriteLine($"[CHANGED] {filePath}");
    }

    public void OnFileDeleted(string filePath)
    {
        Console.WriteLine($"[DELETED] {filePath}");
    }
}


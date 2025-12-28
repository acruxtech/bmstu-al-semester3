namespace Patterns.Singleton;

/// <summary>
/// Класс для генерации случайных чисел с использованием паттерна Singleton
/// Потокобезопасная реализация
/// </summary>
public sealed class SingleRandomizer
{
    private static SingleRandomizer? _instance;
    private static readonly object _lockObject = new();
    private readonly Random _random;
    private readonly object _randomLock = new();

    /// <summary>
    /// Приватный конструктор для предотвращения создания экземпляров извне
    /// </summary>
    private SingleRandomizer()
    {
        _random = new Random();
    }

    /// <summary>
    /// Получает единственный экземпляр класса (ленивая инициализация с двойной проверкой блокировки)
    /// </summary>
    public static SingleRandomizer Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new SingleRandomizer();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Возвращает следующее случайное число типа int
    /// </summary>
    public int Next()
    {
        lock (_randomLock)
        {
            return _random.Next();
        }
    }

    /// <summary>
    /// Возвращает следующее случайное число типа int в диапазоне [0, maxValue)
    /// </summary>
    public int Next(int maxValue)
    {
        if (maxValue < 0)
            throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue must be non-negative");

        lock (_randomLock)
        {
            return _random.Next(maxValue);
        }
    }

    /// <summary>
    /// Возвращает следующее случайное число типа int в диапазоне [minValue, maxValue)
    /// </summary>
    public int Next(int minValue, int maxValue)
    {
        if (minValue > maxValue)
            throw new ArgumentException("minValue must be less than or equal to maxValue");

        lock (_randomLock)
        {
            return _random.Next(minValue, maxValue);
        }
    }

    /// <summary>
    /// Возвращает следующее случайное число типа double в диапазоне [0.0, 1.0)
    /// </summary>
    public double NextDouble()
    {
        lock (_randomLock)
        {
            return _random.NextDouble();
        }
    }

    /// <summary>
    /// Заполняет массив байтов случайными значениями
    /// </summary>
    public void NextBytes(byte[] buffer)
    {
        if (buffer == null)
            throw new ArgumentNullException(nameof(buffer));

        lock (_randomLock)
        {
            _random.NextBytes(buffer);
        }
    }
}


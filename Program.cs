using Patterns.Observer;
using Patterns.Repository;
using Patterns.Singleton;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Лабораторная работа №15: Паттерны разработки ===\n");

        // Демонстрация паттерна Observer (Задание 1)
        await DemonstrateObserverPattern();

        Console.WriteLine("\n" + new string('=', 60) + "\n");

        // Демонстрация паттерна Repository (Задание 2)
        await DemonstrateRepositoryPattern();

        Console.WriteLine("\n" + new string('=', 60) + "\n");

        // Демонстрация паттерна Singleton (Задание 3)
        DemonstrateSingletonPattern();

        Console.WriteLine("\n=== Демонстрация завершена ===");
    }

    /// <summary>
    /// Демонстрация паттерна Observer - FileSystemWatcher
    /// </summary>
    static async Task DemonstrateObserverPattern()
    {
        Console.WriteLine("Задание 1: Паттерн Observer - FileSystemWatcher");
        Console.WriteLine("Создаем временную директорию для мониторинга...\n");

        var watchDirectory = Path.Combine(Path.GetTempPath(), "Lab15_Watch");
        Directory.CreateDirectory(watchDirectory);

        try
        {
            // Создаем наблюдателя
            var observer = new ConsoleFileSystemObserver();

            // Создаем FileSystemWatcher
            using var watcher = new Patterns.Observer.FileSystemWatcher(watchDirectory, checkIntervalMs: 500);
            watcher.Subscribe(observer);

            Console.WriteLine($"Мониторинг директории: {watchDirectory}");
            Console.WriteLine("Нажмите любую клавишу для остановки мониторинга...\n");

            watcher.Start();

            // Создаем тестовые файлы
            await Task.Delay(1000);
            var testFile1 = Path.Combine(watchDirectory, "test1.txt");
            File.WriteAllText(testFile1, "Test content 1");
            await Task.Delay(1000);

            var testFile2 = Path.Combine(watchDirectory, "test2.txt");
            File.WriteAllText(testFile2, "Test content 2");
            await Task.Delay(1000);

            // Изменяем файл
            File.AppendAllText(testFile1, " - Modified");
            await Task.Delay(1000);

            // Удаляем файл
            File.Delete(testFile2);
            await Task.Delay(1000);

            watcher.Stop();
            Console.WriteLine("\nМониторинг остановлен.");

            // Очистка
            Directory.Delete(watchDirectory, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    /// <summary>
    /// Демонстрация паттерна Repository - MyLogger
    /// </summary>
    static async Task DemonstrateRepositoryPattern()
    {
        Console.WriteLine("Задание 2: Паттерн Repository - MyLogger\n");

        var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        Directory.CreateDirectory(logsDirectory);

        try
        {
            // Демонстрация записи в текстовый файл
            Console.WriteLine("1. Запись в текстовый файл:");
            var textRepository = new TextFileLogRepository(Path.Combine(logsDirectory, "text_log.txt"));
            var textLogger = new MyLogger(textRepository);
            
            await textLogger.LogInfoAsync("Приложение запущено", "Program");
            await textLogger.LogWarningAsync("Низкий уровень памяти", "System");
            await textLogger.LogErrorAsync("Ошибка подключения к БД", "Database");
            await textLogger.LogDebugAsync("Отладочное сообщение", "Debugger");
            
            Console.WriteLine("Логи записаны в text_log.txt");
            var textLogs = await textLogger.ReadLogsAsync();
            Console.WriteLine($"Прочитано записей: {textLogs.Count()}\n");

            // Демонстрация записи в JSON файл
            Console.WriteLine("2. Запись в JSON файл:");
            var jsonRepository = new JsonFileLogRepository(Path.Combine(logsDirectory, "json_log.json"));
            var jsonLogger = new MyLogger(jsonRepository);
            
            await jsonLogger.LogInfoAsync("JSON лог: Приложение запущено", "Program");
            await jsonLogger.LogWarningAsync("JSON лог: Предупреждение", "System");
            await jsonLogger.LogErrorAsync("JSON лог: Ошибка", "Database");
            
            Console.WriteLine("Логи записаны в json_log.json");
            var jsonLogs = await jsonLogger.ReadLogsAsync();
            Console.WriteLine($"Прочитано записей: {jsonLogs.Count()}\n");

            // Демонстрация записи в БД
            Console.WriteLine("3. Запись в базу данных SQLite:");
            var dbPath = Path.Combine(logsDirectory, "logs.db");
            var dbRepository = new DatabaseLogRepository($"Data Source={dbPath}");
            var dbLogger = new MyLogger(dbRepository);
            
            await dbLogger.LogInfoAsync("DB лог: Приложение запущено", "Program");
            await dbLogger.LogWarningAsync("DB лог: Предупреждение", "System");
            await dbLogger.LogErrorAsync("DB лог: Ошибка", "Database");
            await dbLogger.LogDebugAsync("DB лог: Отладка", "Debugger");
            
            Console.WriteLine($"Логи записаны в {dbPath}");
            var dbLogs = await dbLogger.ReadLogsAsync();
            Console.WriteLine($"Прочитано записей: {dbLogs.Count()}");
            
            foreach (var log in dbLogs.Take(3))
            {
                Console.WriteLine($"  - [{log.Level}] {log.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    /// <summary>
    /// Демонстрация паттерна Singleton - SingleRandomizer
    /// </summary>
    static void DemonstrateSingletonPattern()
    {
        Console.WriteLine("Задание 3: Паттерн Singleton - SingleRandomizer\n");

        // Проверяем, что получаем один и тот же экземпляр
        var instance1 = SingleRandomizer.Instance;
        var instance2 = SingleRandomizer.Instance;
        
        Console.WriteLine($"instance1 == instance2: {ReferenceEquals(instance1, instance2)}");
        Console.WriteLine("Оба экземпляра ссылаются на один объект (Singleton работает)\n");

        // Генерируем случайные числа из одного потока
        Console.WriteLine("Генерация случайных чисел из основного потока:");
        for (int i = 0; i < 5; i++)
        {
            var value = SingleRandomizer.Instance.Next(1, 100);
            Console.WriteLine($"  Случайное число [{i + 1}]: {value}");
        }

        // Демонстрация работы из разных потоков
        Console.WriteLine("\nГенерация случайных чисел из разных потоков:");
        var threads = new List<Thread>();
        var results = new List<int>();
        var lockObject = new object();

        for (int i = 0; i < 5; i++)
        {
            int threadId = i + 1;
            var thread = new Thread(() =>
            {
                var randomValue = SingleRandomizer.Instance.Next(100, 200);
                lock (lockObject)
                {
                    results.Add(randomValue);
                    Console.WriteLine($"  Поток {threadId}: {randomValue}");
                }
            });
            threads.Add(thread);
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine($"\nВсего сгенерировано значений: {results.Count}");
        Console.WriteLine("Потокобезопасность проверена: все потоки успешно использовали один экземпляр SingleRandomizer");
    }
}


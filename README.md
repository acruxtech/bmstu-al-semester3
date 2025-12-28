# Лабораторная работа №15: Паттерны разработки

## Описание

Реализация трех паттернов проектирования:
1. **Observer (Наблюдатель)** - аналог FileSystemWatcher
2. **Repository (Репозиторий)** - система логирования MyLogger
3. **Singleton (Одиночка)** - потокобезопасный генератор случайных чисел

## Структура проекта

```
.
├── Patterns/
│   ├── Observer/
│   │   ├── IFileSystemObserver.cs      # Интерфейс наблюдателя
│   │   ├── FileSystemWatcher.cs        # Класс для мониторинга файловой системы
│   │   └── ConsoleFileSystemObserver.cs # Реализация наблюдателя для консоли
│   ├── Repository/
│   │   ├── ILogRepository.cs           # Интерфейс репозитория логов
│   │   ├── LogEntry.cs                 # Модель записи лога
│   │   ├── TextFileLogRepository.cs    # Репозиторий для текстовых файлов
│   │   ├── JsonFileLogRepository.cs    # Репозиторий для JSON файлов
│   │   ├── DatabaseLogRepository.cs    # Репозиторий для БД (SQLite)
│   │   └── MyLogger.cs                 # Класс логирования
│   └── Singleton/
│       └── SingleRandomizer.cs         # Потокобезопасный Singleton для ГСЧ
├── Program.cs                           # Демонстрация всех паттернов
└── Lab15.csproj                        # Файл проекта

```

## Задание 1: Паттерн Observer

Реализован аналог `FileSystemWatcher` без обращения к компонентам ОС. Мониторинг директории выполняется по таймеру с проверкой изменений файлов.

### Основные компоненты:
- `IFileSystemObserver` - интерфейс наблюдателя
- `FileSystemWatcher` - класс для мониторинга директории
- `ConsoleFileSystemObserver` - реализация наблюдателя для вывода в консоль

### Использование:
```csharp
var observer = new ConsoleFileSystemObserver();
using var watcher = new Observer.FileSystemWatcher("путь/к/директории", checkIntervalMs: 1000);
watcher.Subscribe(observer);
watcher.Start();
```

## Задание 2: Паттерн Repository

Реализован класс `MyLogger` с возможностью записи логов в различные хранилища через паттерн Repository.

### Поддерживаемые репозитории:
1. **TextFileLogRepository** - запись в текстовый файл построчно
2. **JsonFileLogRepository** - запись в JSON файл с сохранением разметки
3. **DatabaseLogRepository** - запись в базу данных SQLite

### Использование:
```csharp
// Текстовый файл
var textRepo = new TextFileLogRepository("logs.txt");
var logger = new MyLogger(textRepo);
await logger.LogInfoAsync("Сообщение", "Source");

// JSON файл
var jsonRepo = new JsonFileLogRepository("logs.json");
var jsonLogger = new MyLogger(jsonRepo);
await jsonLogger.LogErrorAsync("Ошибка", "Database");

// База данных
var dbRepo = new DatabaseLogRepository("Data Source=logs.db");
var dbLogger = new MyLogger(dbRepo);
await dbLogger.LogWarningAsync("Предупреждение", "System");
```

## Задание 3: Паттерн Singleton

Реализован класс `SingleRandomizer` для генерации случайных чисел с использованием паттерна Singleton. Класс потокобезопасен и может использоваться из разных потоков одновременно.

### Особенности:
- Ленивая инициализация с двойной проверкой блокировки
- Потокобезопасная генерация случайных чисел
- Единственный экземпляр на все приложение

### Использование:
```csharp
// Получение экземпляра
var randomizer = SingleRandomizer.Instance;

// Генерация случайных чисел
int value1 = randomizer.Next(1, 100);
int value2 = randomizer.Next(50);
double value3 = randomizer.NextDouble();
```

## Запуск проекта

```bash
dotnet restore
dotnet run
```

## Требования

- .NET 8.0 SDK
- Пакеты NuGet:
  - Microsoft.Data.Sqlite (8.0.0)
  - Newtonsoft.Json (13.0.3)

## Результаты выполнения

При запуске программа демонстрирует работу всех трех паттернов:
1. Мониторинг файловой системы с выводом событий в консоль
2. Запись логов в текстовый файл, JSON файл и базу данных
3. Генерация случайных чисел из одного и нескольких потоков

Все логи сохраняются в директории `Logs/` в корне проекта.


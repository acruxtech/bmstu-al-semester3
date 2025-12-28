using Microsoft.Data.Sqlite;
using System.Text;

namespace Patterns.Repository;

/// <summary>
/// Репозиторий для записи логов в базу данных SQLite
/// </summary>
public class DatabaseLogRepository : ILogRepository
{
    private readonly string _connectionString;
    private readonly object _lockObject = new();

    public DatabaseLogRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        lock (_lockObject)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var createTableCommand = @"
                CREATE TABLE IF NOT EXISTS Logs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Timestamp TEXT NOT NULL,
                    Level TEXT NOT NULL,
                    Message TEXT NOT NULL,
                    Source TEXT
                )";

            using var command = new SqliteCommand(createTableCommand, connection);
            command.ExecuteNonQuery();
        }
    }

    public async Task WriteLogAsync(LogEntry logEntry)
    {
        if (logEntry == null)
            throw new ArgumentNullException(nameof(logEntry));

        await Task.Run(() =>
        {
            lock (_lockObject)
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                var insertCommand = @"
                    INSERT INTO Logs (Timestamp, Level, Message, Source)
                    VALUES (@Timestamp, @Level, @Message, @Source)";

                using var command = new SqliteCommand(insertCommand, connection);
                command.Parameters.AddWithValue("@Timestamp", logEntry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@Level", logEntry.Level);
                command.Parameters.AddWithValue("@Message", logEntry.Message);
                command.Parameters.AddWithValue("@Source", (object?)logEntry.Source ?? DBNull.Value);

                command.ExecuteNonQuery();
            }
        });
    }

    public async Task<IEnumerable<LogEntry>> ReadLogsAsync()
    {
        return await Task.Run(() =>
        {
            var logs = new List<LogEntry>();

            lock (_lockObject)
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                var selectCommand = "SELECT Timestamp, Level, Message, Source FROM Logs ORDER BY Timestamp DESC";

                using var command = new SqliteCommand(selectCommand, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var logEntry = new LogEntry
                    {
                        Timestamp = DateTime.Parse(reader.GetString(0)),
                        Level = reader.GetString(1),
                        Message = reader.GetString(2),
                        Source = reader.IsDBNull(3) ? null : reader.GetString(3)
                    };

                    logs.Add(logEntry);
                }
            }

            return logs;
        });
    }
}


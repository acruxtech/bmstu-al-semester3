using System.Net.Sockets;
using System.Text;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var host = "127.0.0.1";
        var port = 5001;

        var ticker = Prompt("Введите тикер: ");
        if (string.IsNullOrWhiteSpace(ticker))
        {
            Console.WriteLine("Тикер не задан.");
            return;
        }

        using var client = new TcpClient();
        await client.ConnectAsync(host, port);
        using var stream = client.GetStream();
        using var writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };
        using var reader = new StreamReader(stream, new UTF8Encoding(false), false, leaveOpen: true);

        await writer.WriteLineAsync(ticker.Trim().ToUpperInvariant());
        var response = await reader.ReadLineAsync();
        Console.WriteLine(response ?? "Нет ответа");
    }

    private static string Prompt(string message)
    {
        Console.Write(message);
        return Console.ReadLine() ?? string.Empty;
    }
}



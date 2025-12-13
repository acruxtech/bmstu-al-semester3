using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.EntityFrameworkCore;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var port = 5001;
        var dbPath = ResolveDbPath();

        var listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"[Server] Listening on 0.0.0.0:{port}. Press Ctrl+C to stop.");

        try
        {
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client, dbPath));
            }
        }
        finally
        {
            listener.Stop();
            Console.WriteLine("[Server] Stopped.");
        }
    }

    private static string ResolveDbPath()
    {
        var lab10Db = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "../../../../Lab10.Stocks/bin/Debug/net8.0/stocks.db"));
        var localDb = Path.Combine(AppContext.BaseDirectory, "stocks.db");
        if (!File.Exists(localDb) && File.Exists(lab10Db))
        {
            File.Copy(lab10Db, localDb, overwrite: false);
            Console.WriteLine($"[Server] Copied DB to: {localDb}");
        }
        return File.Exists(localDb) ? localDb : lab10Db;
    }

    private static async Task HandleClientAsync(TcpClient client, string dbPath)
    {
        using var c = client;
        using var stream = c.GetStream();
        using var reader = new StreamReader(stream, new UTF8Encoding(false), false, leaveOpen: true);
        using var writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };
        try
        {
            var line = await reader.ReadLineAsync();
            var ticker = line?.Trim().ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(ticker))
            {
                await writer.WriteLineAsync("ERR EmptyTicker");
                return;
            }

            var response = await QueryLastPriceAsync(dbPath, ticker);
            await writer.WriteLineAsync(response);
        }
        catch (Exception ex)
        {
            try { await writer.WriteLineAsync("ERR " + ex.GetType().Name); } catch { /* ignore */ }
        }
    }

    private static async Task<string> QueryLastPriceAsync(string dbPath, string ticker)
    {
        await using var db = new StocksDb(dbPath);
        var t = await db.Tickers.FirstOrDefaultAsync(x => x.TickerSymbol == ticker);
        if (t == null) return "NOT_FOUND";

        var lastPrice = await db.Prices
            .Where(p => p.TickerId == t.Id)
            .OrderByDescending(p => p.Date)
            .Select(p => (double?)p.PriceValue)
            .FirstOrDefaultAsync();

        if (lastPrice is null) return "NO_PRICE";
        return lastPrice.Value.ToString("0.####", CultureInfo.InvariantCulture);
    }
}



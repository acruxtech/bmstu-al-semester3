using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

internal static class Program
{
    private static readonly HttpClient Http = new HttpClient();

    public static async Task Main(string[] args)
    {
        Http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; Lab10Stocks/1.0)");
        var baseDir = AppContext.BaseDirectory;
        var tickersPath = Path.Combine(baseDir, "tickers.txt");
        if (!File.Exists(tickersPath))
        {
            await File.WriteAllTextAsync(tickersPath, "AAPL\nMSFT\nGOOGL\n", Encoding.UTF8);
        }

        using var db = new StocksDb();
        db.Database.EnsureCreated();

        var tickers = File.ReadAllLines(tickersPath, Encoding.UTF8)
            .Select(t => t.Trim().ToUpperInvariant())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Distinct()
            .ToArray();

        foreach (var t in tickers)
        {
            var ticker = await db.Tickers.FirstOrDefaultAsync(x => x.TickerSymbol == t);
            if (ticker == null)
            {
                ticker = new Ticker { TickerSymbol = t };
                db.Tickers.Add(ticker);
                await db.SaveChangesAsync();
            }

            await IngestPricesAsync(db, ticker);
            await UpsertTodayConditionAsync(db, ticker);
        }

        Console.WriteLine("База обновлена. Введите тикер для проверки (или пусто для выхода):");
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine()?.Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(input)) break;
            await PrintConditionAsync(db, input);
        }
    }

    private static async Task IngestPricesAsync(StocksDb db, Ticker ticker)
    {
        var url = $"https://query1.finance.yahoo.com/v8/finance/chart/{Uri.EscapeDataString(ticker.TickerSymbol)}?range=1mo&interval=1d";
        var json = await Http.GetStringAsync(url);

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement.GetProperty("chart").GetProperty("result")[0];
        var timestamps = root.GetProperty("timestamp").EnumerateArray().ToArray();
        var closes = root.GetProperty("indicators").GetProperty("quote")[0].GetProperty("close").EnumerateArray().ToArray();

        var len = Math.Min(timestamps.Length, closes.Length);
        var existingDates = await db.Prices
            .Where(p => p.TickerId == ticker.Id)
            .Select(p => p.Date)
            .ToListAsync();
        var existing = new HashSet<string>(existingDates);
        for (int i = 0; i < len; i++)
        {
            var tsEl = timestamps[i];
            var closeEl = closes[i];
            if (closeEl.ValueKind != JsonValueKind.Number) continue;
            var close = closeEl.GetDouble();
            var ts = DateTimeOffset.FromUnixTimeSeconds(tsEl.GetInt64()).UtcDateTime.Date;
            var dateStr = ts.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (existing.Add(dateStr))
            {
                db.Prices.Add(new Price { TickerId = ticker.Id, PriceValue = close, Date = dateStr });
            }
        }
        await db.SaveChangesAsync();
    }

    private static async Task UpsertTodayConditionAsync(StocksDb db, Ticker ticker)
    {
        var lastTwo = await db.Prices
            .Where(p => p.TickerId == ticker.Id)
            .OrderByDescending(p => p.Date)
            .Take(2)
            .ToListAsync();
        if (lastTwo.Count < 2) return;

        var last = lastTwo[0].PriceValue;
        var prev = lastTwo[1].PriceValue;
        var state = last > prev ? "выросла" : last < prev ? "упала" : "без изменений";

        var cond = await db.TodaysConditions.FirstOrDefaultAsync(c => c.TickerId == ticker.Id);
        var nowIso = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture);
        if (cond == null)
        {
            db.TodaysConditions.Add(new TodaysCondition { TickerId = ticker.Id, State = state, UpdatedAt = nowIso });
        }
        else
        {
            cond.State = state;
            cond.UpdatedAt = nowIso;
        }
        await db.SaveChangesAsync();
    }

    private static async Task PrintConditionAsync(StocksDb db, string ticker)
    {
        var t = await db.Tickers.FirstOrDefaultAsync(x => x.TickerSymbol == ticker);
        if (t == null)
        {
            Console.WriteLine("Нет данных для этого тикера.");
            return;
        }
        var state = await db.TodaysConditions.Where(c => c.TickerId == t.Id).Select(c => c.State).FirstOrDefaultAsync();
        if (state == null)
        {
            Console.WriteLine("Нет данных для этого тикера.");
        }
        else
        {
            Console.WriteLine($"Акция {ticker}: {state}");
        }
    }
}



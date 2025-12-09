using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

internal static class Program
{
    private static readonly HttpClient Http = CreateHttp();

    public static async Task Main(string[] args)
    {
        var baseDir = AppContext.BaseDirectory;
        var tickersPath = Path.Combine(baseDir, "tickers.txt");
        if (!File.Exists(tickersPath))
        {
            await File.WriteAllTextAsync(tickersPath, "AAPL\nMSFT\nGOOGL\n", Encoding.UTF8);
        }

        var tickers = File.ReadAllLines(tickersPath, Encoding.UTF8)
            .Select(t => t.Trim().ToUpperInvariant())
            .Where(t => t.Length > 0)
            .Distinct()
            .ToArray();

        var resultPath = Path.Combine(baseDir, "result.txt");
        var lines = new List<string>();
        foreach (var t in tickers)
        {
            var avg = await GetMonthAverageAsync(t);
            var line = avg.HasValue ? $"{t}:{avg.Value.ToString("F4", CultureInfo.InvariantCulture)}" : $"{t}:N/A";
            Console.WriteLine(line);
            lines.Add(line);
        }
        await File.WriteAllLinesAsync(resultPath, lines, Encoding.UTF8);
        Console.WriteLine($"Готово. Результаты в {resultPath}");
    }

    private static HttpClient CreateHttp()
    {
        var c = new HttpClient();
        c.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; Lab09Stocks-Simple/1.0)");
        return c;
    }

    private static async Task<double?> GetMonthAverageAsync(string ticker)
    {
        var url = $"https://query1.finance.yahoo.com/v8/finance/chart/{Uri.EscapeDataString(ticker)}?range=1mo&interval=1d";
        var json = await Http.GetStringAsync(url);

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement
            .GetProperty("chart")
            .GetProperty("result")[0]
            .GetProperty("indicators")
            .GetProperty("quote")[0];

        double sum = 0;
        int count = 0;
        var hIter = root.GetProperty("high").EnumerateArray().GetEnumerator();
        var lIter = root.GetProperty("low").EnumerateArray().GetEnumerator();
        while (hIter.MoveNext() && lIter.MoveNext())
        {
            var h = hIter.Current;
            var l = lIter.Current;
            if (h.ValueKind == JsonValueKind.Number && l.ValueKind == JsonValueKind.Number)
            {
                sum += (h.GetDouble() + l.GetDouble()) / 2.0;
                count++;
            }
        }
        if (count > 0) return sum / count;
        return null;
    }
}

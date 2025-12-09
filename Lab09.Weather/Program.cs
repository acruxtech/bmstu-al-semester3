using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

internal static class Program
{
    private static readonly HttpClient Http = new HttpClient();

    public static async Task Main(string[] args)
    {
        var baseDir = AppContext.BaseDirectory;
        var cities = File.ReadAllLines(Path.Combine(baseDir, "city.txt"), Encoding.UTF8)
            .Select(c => c.Trim())
            .Where(c => c.Length > 0)
            .Distinct()
            .ToArray();

        Console.WriteLine("Список городов:");
        foreach (var c in cities) Console.WriteLine($"- {c}");
        Console.WriteLine("Нажмите Enter для запроса текущей погоды...");
        Console.ReadLine();

        var lines = new List<string>();
        foreach (var city in cities)
        {
            lines.Add(await GetWeatherLineAsync(city));
        }

        var resultPath = Path.Combine(baseDir, "weather.txt");
        await File.WriteAllLinesAsync(resultPath, lines, Encoding.UTF8);
        Console.WriteLine($"Готово. Результаты записаны в {resultPath}");
    }

    private static async Task<string> GetWeatherLineAsync(string city)
    {
        var geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(city)}&count=1&language=ru&format=json";
        var geoJson = await Http.GetStringAsync(geoUrl);
        using var geoDoc = JsonDocument.Parse(geoJson);

        var results = geoDoc.RootElement.TryGetProperty("results", out var res) ? res : default;
        if (results.ValueKind != JsonValueKind.Array || results.GetArrayLength() == 0)
        {
            return $"{city}: нет данных";
        }

        var first = results[0];
        var lat = first.GetProperty("latitude").GetDouble();
        var lon = first.GetProperty("longitude").GetDouble();

        var inv = System.Globalization.CultureInfo.InvariantCulture;
        var wUrl =
            $"https://api.open-meteo.com/v1/forecast?latitude={lat.ToString(inv)}&longitude={lon.ToString(inv)}&current=temperature_2m,precipitation,wind_speed_10m";
        var wJson = await Http.GetStringAsync(wUrl);
        using var wDoc = JsonDocument.Parse(wJson);
        var cur = wDoc.RootElement.GetProperty("current");

        var t = cur.GetProperty("temperature_2m").GetDouble();
        var w = cur.GetProperty("wind_speed_10m").GetDouble();
        var p = cur.GetProperty("precipitation").GetDouble();

        return $"{city}: {t}°C, ветер {w} м/с, осадки {p} мм";
    }
}
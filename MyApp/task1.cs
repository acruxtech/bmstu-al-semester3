using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace Task1
{
    struct Weather
    {
        public string Country { get; init; }
        public string Name { get; init; }
        public double Temp { get; init; }
        public string Description { get; init; }
    }

    class Program
    {
        private const int n = 50;

        private static readonly string[] TargetDescriptions = { "clear sky", "rain", "few clouds" };

        static void Main(string[] args)
        {
            var apiKey = "e1344df388bec043c11c1a1642bffe68";

            using var httpClient = new HttpClient();
            var random = new Random();
            var weatherSamples = new List<Weather>(n);

            while (weatherSamples.Count < n)
            {
                var lat = NextDoubleInRange(random, -90d, 90d);
                var lon = NextDoubleInRange(random, -180d, 180d); 
                var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat.ToString()}&lon={lon.ToString()}&units=metric&appid={apiKey}";

            
                using var response = httpClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    continue;
                }

                var jsonContent = response.Content.ReadAsStringAsync().Result;
                using var jsonDocument = JsonDocument.Parse(jsonContent);

                if (!TryMapToWeather(jsonDocument, out var weather))
                {
                    continue;
                }

                weatherSamples.Add(weather);
                Console.WriteLine($"{weatherSamples.Count,2}: {weather.Country,-3} | {weather.Name,-25} | {weather.Temp,6:F1}°C | {weather.Description}");
            }

            Console.WriteLine("\nСтатистика:\n");
            DisplayCountryTemperatureExtremes(weatherSamples);
            DisplayAverageTemperature(weatherSamples);
            DisplayCountryCount(weatherSamples);
            DisplayFirstMatchingDescriptions(weatherSamples);
        }

        private static double NextDoubleInRange(Random random, double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        private static bool TryMapToWeather(JsonDocument document, out Weather weather)
        {
            weather = default;

            if (!document.RootElement.TryGetProperty("sys", out var sysElement) ||
                !sysElement.TryGetProperty("country", out var countryElement))
            {
                return false;
            }


            if (!document.RootElement.TryGetProperty("name", out var nameElement))
            {
                return false;
            }

            var country = countryElement.GetString()!;
            var name = nameElement.GetString()!;
            var temp = document.RootElement.GetProperty("main").GetProperty("temp").GetDouble();
            var description = document.RootElement.GetProperty("weather")[0].GetProperty("description").GetString() ?? string.Empty;

            weather = new Weather
            {
                Country = country,
                Name = name,
                Temp = temp,
                Description = description
            };

            return true;
        }

        private static void DisplayCountryTemperatureExtremes(IReadOnlyCollection<Weather> samples)
        {
            var maxTemp = samples.MaxBy(w => w.Temp);
            var minTemp = samples.MinBy(w => w.Temp);

            Console.WriteLine($"- Страна с макс. температурой: {maxTemp.Country} ({maxTemp.Temp:F1}°C) в городе {maxTemp.Name}");
            Console.WriteLine($"- Страна с мин. температурой: {minTemp.Country} ({minTemp.Temp:F1}°C) в городе {minTemp.Name}");
        }

        private static void DisplayAverageTemperature(IReadOnlyCollection<Weather> samples)
        {
            var average = samples.Average(w => w.Temp);
            Console.WriteLine($"- Средняя температура: {average:F1}°C");
        }

        private static void DisplayCountryCount(IReadOnlyCollection<Weather> samples)
        {
            var countryCount = samples
                .Select(s => s.Country)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();

            Console.WriteLine($"- Количество стран в наборе данных: {countryCount}");
        }

        private static void DisplayFirstMatchingDescriptions(IEnumerable<Weather> samples)
        {
            foreach (var target in TargetDescriptions)
            {
                var match = samples.FirstOrDefault(w => string.Equals(w.Description, target, StringComparison.OrdinalIgnoreCase));
                if (!match.Equals(default(Weather)))
                {
                    Console.WriteLine($"- Первая запись с описанием '{target}': {match.Country}, {match.Name}");
                }
                else
                {
                    Console.WriteLine($"- Не найдено записей с описанием '{target}'");
                }
            }
        }
    }
}

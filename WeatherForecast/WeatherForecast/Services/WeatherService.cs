using System.Net.Http.Json;
using System.Text.Json;
using WeatherForecast.Models;

namespace WeatherForecast.Services;

/// <summary>
/// Fetches weather data from Open-Meteo (https://open-meteo.com).
/// Completely free – no API key required.
///
/// Flow:
///   1. Geocoding API  → resolve city name to lat/lon
///       GET https://geocoding-api.open-meteo.com/v1/search
///   2. Forecast API   → current + hourly + daily weather
///       GET https://api.open-meteo.com/v1/forecast
/// </summary>
public class WeatherService
{
    private const string GeocodingBaseUrl = "https://geocoding-api.open-meteo.com/v1/search";
    private const string ForecastBaseUrl  = "https://api.open-meteo.com/v1/forecast";

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(20);
    }

    // ── Public API ───────────────────────────────────────────────────────────

    public async Task<WeatherResult> GetForecastAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City name must not be empty.", nameof(city));

        // Step 1: geocode city name → lat/lon ────────────────────────────────
        var geoUrl = $"{GeocodingBaseUrl}?name={Uri.EscapeDataString(city)}&count=1&language=en&format=json";
        GeocodingResponse geoResponse;
        try
        {
            geoResponse = await _httpClient
                              .GetFromJsonAsync<GeocodingResponse>(geoUrl, _jsonOptions)
                          ?? throw new InvalidOperationException("Empty geocoding response.");
        }
        catch (HttpRequestException ex)
        {
            throw new WeatherServiceException(
                $"Network error while geocoding '{city}': {ex.Message}", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new WeatherServiceException(
                "Request timed out. Check your internet connection.", ex);
        }

        if (geoResponse.Results is not { Count: > 0 })
            throw new WeatherServiceException($"City '{city}' not found.");

        var geo       = geoResponse.Results[0];
        var cityLabel = string.IsNullOrEmpty(geo.CountryCode)
            ? geo.Name
            : $"{geo.Name}, {geo.CountryCode}";

        // Step 2: fetch forecast ──────────────────────────────────────────────
        var forecastUrl =
            $"{ForecastBaseUrl}" +
            $"?latitude={geo.Latitude:F4}&longitude={geo.Longitude:F4}" +
            "&current=temperature_2m,apparent_temperature,weather_code,wind_speed_10m,relative_humidity_2m" +
            "&hourly=temperature_2m,weather_code,precipitation_probability" +
            "&daily=weather_code,temperature_2m_max,temperature_2m_min,precipitation_probability_max" +
            "&timezone=auto&forecast_days=7";

        OpenMeteoForecastResponse forecast;
        try
        {
            forecast = await _httpClient
                           .GetFromJsonAsync<OpenMeteoForecastResponse>(forecastUrl, _jsonOptions)
                       ?? throw new InvalidOperationException("Empty forecast response.");
        }
        catch (HttpRequestException ex)
        {
            throw new WeatherServiceException(
                $"Network error while fetching weather for '{city}': {ex.Message}", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new WeatherServiceException(
                "Request timed out. Check your internet connection.", ex);
        }
        catch (JsonException ex)
        {
            throw new WeatherServiceException("Failed to parse weather data.", ex);
        }

        var current = forecast.Current;
        var daily   = forecast.Daily;
        double high = daily.Temperature2mMax.Count > 0 ? Math.Round(daily.Temperature2mMax[0], 1) : current.Temperature2m;
        double low  = daily.Temperature2mMin.Count > 0 ? Math.Round(daily.Temperature2mMin[0], 1) : current.Temperature2m;

        return new WeatherResult
        {
            CityName           = cityLabel,
            CurrentTemperature = Math.Round(current.Temperature2m, 1),
            FeelsLike          = Math.Round(current.ApparentTemperature, 1),
            Description        = WmoDescription(current.WeatherCode),
            CurrentIcon        = WmoIcon(current.WeatherCode),
            Humidity           = current.RelativeHumidity2m,
            WindSpeed          = Math.Round(current.WindSpeed10m, 1),
            HighTemperature    = high,
            LowTemperature     = low,
            HourlyForecasts    = BuildHourlyForecasts(forecast),
            DailyForecasts     = BuildDailyForecasts(forecast)
        };
    }

    // ── Hourly ───────────────────────────────────────────────────────────────

    private static List<HourlyForecast> BuildHourlyForecasts(OpenMeteoForecastResponse forecast)
    {
        var hourly = forecast.Hourly;
        var now    = DateTime.Now;
        var result = new List<HourlyForecast>();

        for (int i = 0; i < hourly.Time.Count && result.Count < 8; i++)
        {
            if (!DateTime.TryParse(hourly.Time[i], out var dt)) continue;
            if (dt < now) continue;  // skip past slots

            result.Add(new HourlyForecast
            {
                DateTime     = dt,
                TimeLabel    = dt.ToString("HH:mm"),
                Temperature  = Math.Round(hourly.Temperature2m[i], 1),
                Description  = WmoDescription(hourly.WeatherCode[i]),
                Icon         = WmoIcon(hourly.WeatherCode[i]),
                PrecipChance = i < hourly.PrecipitationProbability.Count
                               ? hourly.PrecipitationProbability[i] : 0,
                WeatherEmoji = WmoEmoji(hourly.WeatherCode[i])
            });
        }

        // Fallback: first 8 slots if all times were in the past
        if (result.Count == 0)
        {
            for (int i = 0; i < Math.Min(8, hourly.Time.Count); i++)
            {
                if (!DateTime.TryParse(hourly.Time[i], out var dt)) continue;
                result.Add(new HourlyForecast
                {
                    DateTime     = dt,
                    TimeLabel    = dt.ToString("HH:mm"),
                    Temperature  = Math.Round(hourly.Temperature2m[i], 1),
                    Description  = WmoDescription(hourly.WeatherCode[i]),
                    Icon         = WmoIcon(hourly.WeatherCode[i]),
                    PrecipChance = i < hourly.PrecipitationProbability.Count
                                   ? hourly.PrecipitationProbability[i] : 0,
                    WeatherEmoji = WmoEmoji(hourly.WeatherCode[i])
                });
            }
        }

        return result;
    }

    // ── Daily ────────────────────────────────────────────────────────────────

    private static List<DailyForecast> BuildDailyForecasts(OpenMeteoForecastResponse forecast)
    {
        var daily  = forecast.Daily;
        var today  = DateTime.Today;
        var result = new List<DailyForecast>();

        for (int i = 0; i < daily.Time.Count; i++)
        {
            if (!DateOnly.TryParse(daily.Time[i], out var date)) continue;
            var dt     = date.ToDateTime(TimeOnly.MinValue);
            int code   = i < daily.WeatherCode.Count        ? daily.WeatherCode[i]             : 0;
            double max = i < daily.Temperature2mMax.Count   ? Math.Round(daily.Temperature2mMax[i], 1) : 0;
            double min = i < daily.Temperature2mMin.Count   ? Math.Round(daily.Temperature2mMin[i], 1) : 0;
            double avg = Math.Round((max + min) / 2.0, 1);

            result.Add(new DailyForecast
            {
                Date           = dt,
                DayName        = dt.Date == today ? "Today" : dt.ToString("ddd"),
                DateLabel      = dt.ToString("MMM d"),
                AvgTemperature = avg,
                MaxTemperature = max,
                MinTemperature = min,
                Description    = WmoDescription(code),
                Icon           = WmoIcon(code),
                WeatherEmoji   = WmoEmoji(code)
            });
        }

        if (result.Count > 0)
            result.OrderByDescending(d => d.AvgTemperature).First().IsHottestDay = true;

        return result;
    }

    // ── WMO weather-code helpers ─────────────────────────────────────────────

    private static string WmoEmoji(int code) => code switch
    {
        0                                     => "☀️",
        1                                     => "🌤️",
        2                                     => "⛅",
        3                                     => "☁️",
        45 or 48                              => "🌫️",
        51 or 53 or 55 or 56 or 57            => "🌦️",
        61 or 63 or 65 or 66 or 67            => "🌧️",
        71 or 73 or 75 or 77 or 85 or 86      => "❄️",
        80 or 81 or 82                        => "🌧️",
        95 or 96 or 99                        => "⛈️",
        _                                     => "🌤️"
    };

    private static string WmoDescription(int code) => code switch
    {
        0        => "Clear sky",
        1        => "Mainly clear",
        2        => "Partly cloudy",
        3        => "Overcast",
        45       => "Foggy",
        48       => "Icy fog",
        51       => "Light drizzle",
        53       => "Moderate drizzle",
        55       => "Dense drizzle",
        56 or 57 => "Freezing drizzle",
        61       => "Slight rain",
        63       => "Moderate rain",
        65       => "Heavy rain",
        66 or 67 => "Freezing rain",
        71       => "Slight snow",
        73       => "Moderate snow",
        75       => "Heavy snow",
        77       => "Snow grains",
        80       => "Slight showers",
        81       => "Moderate showers",
        82       => "Violent showers",
        85 or 86 => "Snow showers",
        95       => "Thunderstorm",
        96 or 99 => "Thunderstorm with hail",
        _        => "Unknown"
    };

    /// <summary>
    /// Maps WMO codes to OWM-style icon strings so the existing
    /// icon→emoji switch in WeatherViewModel continues to work unchanged.
    /// </summary>
    private static string WmoIcon(int code) => code switch
    {
        0                                => "01d",
        1                                => "02d",
        2                                => "03d",
        3                                => "04d",
        45 or 48                         => "50d",
        51 or 53 or 55 or 56 or 57       => "09d",
        61 or 63 or 65 or 66 or 67       => "10d",
        71 or 73 or 75 or 77 or 85 or 86 => "13d",
        80 or 81 or 82                   => "09d",
        95 or 96 or 99                   => "11d",
        _                                => "02d"
    };
}

// ── Result + exception types ──────────────────────────────────────────────────

public class WeatherResult
{
    public string CityName { get; init; } = string.Empty;
    public double CurrentTemperature { get; init; }
    public double FeelsLike { get; init; }
    public string Description { get; init; } = string.Empty;
    public string CurrentIcon { get; init; } = "01d";
    public int Humidity { get; init; }
    public double WindSpeed { get; init; }
    public double HighTemperature { get; init; }
    public double LowTemperature { get; init; }
    public List<HourlyForecast> HourlyForecasts { get; init; } = [];
    public List<DailyForecast> DailyForecasts { get; init; } = [];
}

public class WeatherServiceException : Exception
{
    public WeatherServiceException(string message) : base(message) { }
    public WeatherServiceException(string message, Exception inner) : base(message, inner) { }
}

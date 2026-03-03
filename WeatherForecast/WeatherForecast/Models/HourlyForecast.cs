namespace WeatherForecast.Models;

/// <summary>
/// Processed model for a single hourly (3-hour) forecast slot —
/// used by the Today tab chart and horizontal list.
/// </summary>
public class HourlyForecast
{
    public DateTime DateTime { get; set; }

    /// <summary>Short label shown on chart x-axis  e.g. "09:00"</summary>
    public string TimeLabel { get; set; } = string.Empty;

    /// <summary>Temperature in Celsius</summary>
    public double Temperature { get; set; }

    public string Description { get; set; } = string.Empty;

    /// <summary>OpenWeatherMap icon code e.g. "10d"</summary>
    public string Icon { get; set; } = string.Empty;

    public string IconUrl => $"https://openweathermap.org/img/wn/{Icon}@2x.png";

    public int Humidity { get; set; }

    /// <summary>Wind speed m/s</summary>
    public double WindSpeed { get; set; }

    /// <summary>Precipitation probability 0-100</summary>
    public int PrecipChance { get; set; }

    /// <summary>Unicode emoji approximating the weather condition</summary>
    public string WeatherEmoji { get; set; } = "🌤️";

    /// <summary>Formatted temperature with degree symbol</summary>
    public string TempDisplay => $"{Temperature:F0}°";

    /// <summary>Formatted precipitation probability</summary>
    public string PrecipDisplay => $"{PrecipChance}%";
}

namespace WeatherForecast.Models;

/// <summary>
/// Aggregated daily forecast model — one entry per calendar day,
/// derived by grouping the 3-hourly API results.
/// </summary>
public class DailyForecast
{
    public DateTime Date { get; set; }

    /// <summary>Short day name for list display  e.g. "Mon"</summary>
    public string DayName { get; set; } = string.Empty;

    /// <summary>Formatted date  e.g. "Mar 3"</summary>
    public string DateLabel { get; set; } = string.Empty;

    /// <summary>Average temperature across all slots for this day</summary>
    public double AvgTemperature { get; set; }

    public double MaxTemperature { get; set; }
    public double MinTemperature { get; set; }

    /// <summary>Most common weather description for the day</summary>
    public string Description { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;
    public string IconUrl => $"https://openweathermap.org/img/wn/{Icon}@2x.png";

    public string WeatherEmoji { get; set; } = "🌤️";

    /// <summary>True for the day that has the highest average temperature</summary>
    public bool IsHottestDay { get; set; }

    /// <summary>Formatted average temp  e.g. "22°"</summary>
    public string AvgTempDisplay => $"{AvgTemperature:F0}°";

    /// <summary>Formatted high / low  e.g. "H: 25° / L: 17°"</summary>
    public string HighLowDisplay => $"H:{MaxTemperature:F0}° / L:{MinTemperature:F0}°";

    /// <summary>Badge text shown on the hottest day card; empty string hides the badge.</summary>
    public string BadgeText => IsHottestDay ? "🔥 HOT" : string.Empty;
}

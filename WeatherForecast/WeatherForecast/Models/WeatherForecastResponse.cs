using System.Text.Json.Serialization;

namespace WeatherForecast.Models;

// ────────────────────────────────────────────────────────────────────────────
//  Open-Meteo response models  (https://open-meteo.com)
//  No API key required.
//
//  Geocoding API:  GET https://geocoding-api.open-meteo.com/v1/search
//  Forecast API:   GET https://api.open-meteo.com/v1/forecast
// ────────────────────────────────────────────────────────────────────────────

// ── Geocoding ────────────────────────────────────────────────────────────────

public class GeocodingResponse
{
    [JsonPropertyName("results")]
    public List<GeocodingResult>? Results { get; set; }
}

public class GeocodingResult
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; } = string.Empty;
}

// ── Forecast ─────────────────────────────────────────────────────────────────

public class OpenMeteoForecastResponse
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = string.Empty;

    [JsonPropertyName("current")]
    public OpenMeteoCurrent Current { get; set; } = new();

    [JsonPropertyName("hourly")]
    public OpenMeteoHourly Hourly { get; set; } = new();

    [JsonPropertyName("daily")]
    public OpenMeteoDaily Daily { get; set; } = new();
}

public class OpenMeteoCurrent
{
    [JsonPropertyName("time")]
    public string Time { get; set; } = string.Empty;

    [JsonPropertyName("temperature_2m")]
    public double Temperature2m { get; set; }

    [JsonPropertyName("apparent_temperature")]
    public double ApparentTemperature { get; set; }

    [JsonPropertyName("weather_code")]
    public int WeatherCode { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public double WindSpeed10m { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public int RelativeHumidity2m { get; set; }
}

public class OpenMeteoHourly
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; } = [];

    [JsonPropertyName("temperature_2m")]
    public List<double> Temperature2m { get; set; } = [];

    [JsonPropertyName("weather_code")]
    public List<int> WeatherCode { get; set; } = [];

    [JsonPropertyName("precipitation_probability")]
    public List<int> PrecipitationProbability { get; set; } = [];
}

public class OpenMeteoDaily
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; } = [];

    [JsonPropertyName("weather_code")]
    public List<int> WeatherCode { get; set; } = [];

    [JsonPropertyName("temperature_2m_max")]
    public List<double> Temperature2mMax { get; set; } = [];

    [JsonPropertyName("temperature_2m_min")]
    public List<double> Temperature2mMin { get; set; } = [];

    [JsonPropertyName("precipitation_probability_max")]
    public List<int> PrecipitationProbabilityMax { get; set; } = [];
}





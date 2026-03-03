using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WeatherForecast.Models;
using WeatherForecast.Services;

namespace WeatherForecast.ViewModels;

/// <summary>
/// ViewModel for WeatherDashboardPage.
/// Implements INotifyPropertyChanged manually for full transparency.
/// All UI updates are marshalled to the main thread.
/// </summary>
public class WeatherViewModel : INotifyPropertyChanged
{
    // ── Dependencies ─────────────────────────────────────────────────────────
    private readonly WeatherService _weatherService;

    // ── Commands ─────────────────────────────────────────────────────────────
    public ICommand SearchCommand { get; }
    public ICommand LoadWeatherCommand { get; }
    public ICommand PullToRefreshCommand { get; }

    // ── Constructor ──────────────────────────────────────────────────────────
    public WeatherViewModel(WeatherService weatherService)
    {
        _weatherService = weatherService;

        SearchCommand = new Command(async () => await FetchWeatherAsync(), CanSearch);
        LoadWeatherCommand = new Command<string>(async city => await FetchWeatherAsync(city));
        PullToRefreshCommand = new Command(async () =>
        {
            await FetchWeatherAsync();
            IsRefreshing = false;
        });

        // Seed popular city suggestions
        CitySuggestions = new List<string>
        {
            "London", "New York", "Tokyo", "Sydney", "Paris",
            "Dubai", "Singapore", "Toronto", "Berlin", "Mumbai",
            "Los Angeles", "Chicago", "Seoul", "Amsterdam", "Madrid"
        };

        // Note: the initial weather load is intentionally NOT started here.
        // Kicking off a network call via Task.Run inside a constructor is
        // unsafe on Android – the MAUI activity may not be fully initialized,
        // which can deadlock the main-thread dispatcher and freeze the splash
        // screen.  Call InitializeAsync() from the page's OnAppearing instead.
    }

    /// <summary>
    /// Call once from the page's OnAppearing to load the default city.
    /// Safe to call again on subsequent appearances – the guard on IsLoading
    /// prevents redundant fetches while data is already present.
    /// </summary>
    public async Task InitializeAsync()
    {
        // Only load once – skip if we already have data or a fetch is in progress.
        if (IsLoading || CityName != "—") return;
        await FetchWeatherAsync(SelectedCity);
    }

    // ──────────────────────────────────────────────────────────────────────────
    //  Bindable Properties
    // ──────────────────────────────────────────────────────────────────────────

    // Search / input ──────────────────────────────────────────────────────────

    private string _selectedCity = "London";
    public string SelectedCity
    {
        get => _selectedCity;
        set
        {
            if (SetProperty(ref _selectedCity, value))
                ((Command)SearchCommand).ChangeCanExecute();
        }
    }

    private List<string> _citySuggestions = [];
    public List<string> CitySuggestions
    {
        get => _citySuggestions;
        set => SetProperty(ref _citySuggestions, value);
    }

    // State ───────────────────────────────────────────────────────────────────

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (SetProperty(ref _isLoading, value))
                OnPropertyChanged(nameof(IsContentVisible));
        }
    }

    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (SetProperty(ref _errorMessage, value))
                OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool IsContentVisible => !IsLoading && !HasError;

    private string _lastUpdated = string.Empty;
    public string LastUpdated
    {
        get => _lastUpdated;
        set => SetProperty(ref _lastUpdated, value);
    }

    // Current weather ─────────────────────────────────────────────────────────

    private string _cityName = "—";
    public string CityName
    {
        get => _cityName;
        set => SetProperty(ref _cityName, value);
    }

    private double _currentTemperature;
    public double CurrentTemperature
    {
        get => _currentTemperature;
        set
        {
            if (SetProperty(ref _currentTemperature, value))
                OnPropertyChanged(nameof(CurrentTempDisplay));
        }
    }

    public string CurrentTempDisplay => $"{CurrentTemperature:F0}°C";

    private double _feelsLike;
    public double FeelsLike
    {
        get => _feelsLike;
        set
        {
            if (SetProperty(ref _feelsLike, value))
                OnPropertyChanged(nameof(FeelsLikeDisplay));
        }
    }

    public string FeelsLikeDisplay => $"Feels like {FeelsLike:F0}°C";

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private string _currentWeatherEmoji = "🌤️";
    public string CurrentWeatherEmoji
    {
        get => _currentWeatherEmoji;
        set => SetProperty(ref _currentWeatherEmoji, value);
    }

    private double _highTemperature;
    public double HighTemperature
    {
        get => _highTemperature;
        set
        {
            if (SetProperty(ref _highTemperature, value))
                OnPropertyChanged(nameof(HighTempDisplay));
        }
    }

    public string HighTempDisplay => $"H: {HighTemperature:F0}°";

    private double _lowTemperature;
    public double LowTemperature
    {
        get => _lowTemperature;
        set
        {
            if (SetProperty(ref _lowTemperature, value))
            {
                OnPropertyChanged(nameof(LowTempDisplay));
                OnPropertyChanged(nameof(TempProgressValue));
            }
        }
    }

    public string LowTempDisplay => $"L: {LowTemperature:F0}°";

    // Progress bar: position of current temp between today's min/max ──────────

    /// <summary>Value 0–100 representing current temp position between low/high.</summary>
    public double TempProgressValue
    {
        get
        {
            double range = HighTemperature - LowTemperature;
            if (range <= 0) return 50;
            double progress = (CurrentTemperature - LowTemperature) / range * 100.0;
            return Math.Clamp(progress, 0, 100);
        }
    }

    // Gauges ──────────────────────────────────────────────────────────────────

    private double _humidity;
    public double Humidity
    {
        get => _humidity;
        set
        {
            if (SetProperty(ref _humidity, value))
                OnPropertyChanged(nameof(HumidityDisplay));
        }
    }

    public string HumidityDisplay => $"{Humidity:F0}%";

    private double _windSpeed;
    public double WindSpeed
    {
        get => _windSpeed;
        set
        {
            if (SetProperty(ref _windSpeed, value))
                OnPropertyChanged(nameof(WindSpeedDisplay));
        }
    }

    public string WindSpeedDisplay => $"{WindSpeed:F1} m/s";

    // Collections ─────────────────────────────────────────────────────────────

    private ObservableCollection<HourlyForecast> _hourlyForecasts = [];
    public ObservableCollection<HourlyForecast> HourlyForecasts
    {
        get => _hourlyForecasts;
        set => SetProperty(ref _hourlyForecasts, value);
    }

    private ObservableCollection<DailyForecast> _dailyForecasts = [];
    public ObservableCollection<DailyForecast> DailyForecasts
    {
        get => _dailyForecasts;
        set => SetProperty(ref _dailyForecasts, value);
    }

    // ──────────────────────────────────────────────────────────────────────────
    //  Core fetch logic
    // ──────────────────────────────────────────────────────────────────────────

    public async Task FetchWeatherAsync(string? city = null)
    {
        var targetCity = city ?? SelectedCity;
        if (string.IsNullOrWhiteSpace(targetCity)) return;

        await RunOnMainThread(() =>
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
        });

        try
        {
            var result = await _weatherService.GetForecastAsync(targetCity);
            await ApplyResultAsync(result);
        }
        catch (WeatherServiceException ex)
        {
            await RunOnMainThread(() => ErrorMessage = ex.Message);
        }
        catch (Exception ex)
        {
            await RunOnMainThread(() =>
                ErrorMessage = $"Unexpected error: {ex.Message}");
        }
        finally
        {
            await RunOnMainThread(() => IsLoading = false);
        }
    }

    private async Task ApplyResultAsync(WeatherResult result)
    {
        await RunOnMainThread(() =>
        {
            CityName = result.CityName;
            CurrentTemperature = Math.Round(result.CurrentTemperature, 1);
            FeelsLike = Math.Round(result.FeelsLike, 1);
            Description = result.Description;
            HighTemperature = result.HighTemperature;
            LowTemperature = result.LowTemperature;
            Humidity = result.Humidity;
            WindSpeed = result.WindSpeed;
            LastUpdated = $"Updated {DateTime.Now:HH:mm}";

            // Map icon to emoji for the current condition
            CurrentWeatherEmoji = GetEmojiForIcon(result.CurrentIcon);

            // Refresh collections
            HourlyForecasts = new ObservableCollection<HourlyForecast>(result.HourlyForecasts);
            DailyForecasts = new ObservableCollection<DailyForecast>(result.DailyForecasts);

            // Notify derived properties that depend on multiple fields
            OnPropertyChanged(nameof(TempProgressValue));
            OnPropertyChanged(nameof(CurrentTempDisplay));
            OnPropertyChanged(nameof(FeelsLikeDisplay));
            OnPropertyChanged(nameof(HighTempDisplay));
            OnPropertyChanged(nameof(LowTempDisplay));
        });
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private bool CanSearch() => !string.IsNullOrWhiteSpace(SelectedCity) && !IsLoading;

    private static string GetEmojiForIcon(string iconCode) => iconCode switch
    {
        "01d" or "01n" => "☀️",
        "02d" or "02n" => "🌤️",
        "03d" or "03n" => "⛅",
        "04d" or "04n" => "☁️",
        "09d" or "09n" => "🌧️",
        "10d" or "10n" => "🌦️",
        "11d" or "11n" => "⛈️",
        "13d" or "13n" => "❄️",
        "50d" or "50n" => "🌫️",
        _ => "🌤️"
    };

    private static Task RunOnMainThread(Action action)
    {
        if (MainThread.IsMainThread)
        {
            action();
            return Task.CompletedTask;
        }
        return MainThread.InvokeOnMainThreadAsync(action);
    }

    // ──────────────────────────────────────────────────────────────────────────
    //  INotifyPropertyChanged
    // ──────────────────────────────────────────────────────────────────────────

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value)) return false;
        backingStore = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

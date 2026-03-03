# WeatherForecast

A beautifully designed .NET MAUI weather app with a dark glassmorphism UI, real-time weather data, hourly/daily forecasts, and rich data visualisations.

## Screenshot

<p align="center">
  <img src="screenshots/screenshot.png" alt="WeatherForecast App" width="320" />
</p>

## Features

- **Today tab** — current conditions with large temperature display, weather emoji, condition badge, feels-like, and daily high/low
- **3 stat cards** — humidity, wind speed, and feels-like temperature at a glance
- **City search** — autocomplete search bar to switch locations instantly
- **Temperature Trend chart** — spline area chart showing the hourly temperature curve for the day
- **Next Hours list** — horizontal scrollable hourly forecast cards
- **Atmosphere gauges** — radial gauges for humidity and wind speed
- **Today's Range** — visual progress bar showing the day's temperature range
- **Next 5 Days tab** — weekly column chart overview plus 5-day forecast rows with condition badges (e.g. "Hottest Day")
- **Pull-to-refresh** — swipe down to reload weather data
- **Loading shimmer** — skeleton shimmer while data is fetching
- **Dark glassmorphism theme** — deep navy gradient background with frosted-glass cards

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/) (net10.0) |
| Language | C# 13 |
| UI Controls | [Syncfusion MAUI](https://www.syncfusion.com/maui-controls) — Charts, ListView, TabView, Gauges, ProgressBar, PullToRefresh, Shimmer, Inputs, Badges |
| Architecture | MVVM (`WeatherViewModel`) |
| Weather Data | [Open-Meteo API](https://open-meteo.com/) via `WeatherService` |
| Targets | iOS · Android · macOS Catalyst · Windows |

## Project Structure

```
WeatherForecast/
├── Models/
│   ├── WeatherForecastResponse.cs   # API response DTOs
│   ├── HourlyForecast.cs            # Hourly data model
│   └── DailyForecast.cs             # Daily data model
├── Services/
│   └── WeatherService.cs            # HTTP weather API client
├── ViewModels/
│   └── WeatherViewModel.cs          # MVVM view model
├── Views/
│   ├── WeatherDashboardPage.xaml    # Main dashboard UI
│   └── WeatherDashboardPage.xaml.cs
├── Resources/
│   ├── Styles/                      # Colours & global styles
│   ├── Fonts/
│   └── Images/
└── Platforms/                       # Android / iOS / macOS / Windows
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- Visual Studio Code with the [.NET MAUI extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-maui)
- Xcode (for iOS / macOS targets)
- Android SDK (for Android target)
- Get a [Syncfusion license key](https://www.syncfusion.com/maui-controls)

## Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/WeatherForecast.git
   cd WeatherForecast
   ```

2. **Register your Syncfusion licence** in `MauiProgram.cs`:
   ```csharp
   Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR_KEY");
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Run on iOS Simulator**
   ```bash
   dotnet build -f net10.0-ios -r iossimulator-arm64
   ```

   **Run on Android**
   ```bash
   dotnet build -f net10.0-android
   ```

## Licence

This project is licensed under the [MIT Licence](LICENSE).

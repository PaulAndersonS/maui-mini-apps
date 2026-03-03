# maui-mini-apps

A collection of sample applications demonstrating [Syncfusion .NET MAUI](https://www.syncfusion.com/maui-controls) controls with real-world use cases and simple, ready-to-use code examples.

---

## 📱 Apps

### 1. Smart Daily Tools

An all-in-one calculator toolkit packed with everyday utilities.

| Tool | Description |
|---|---|
| 🏃 BMI Calculator | Calculate Body Mass Index with category classification and history |
| 🎂 Age Calculator | Find exact age (years, months, days) from a date of birth |
| 💰 EMI Calculator | Compute loan EMI with amortisation breakdown |
| % Percentage Calculator | Quick percentage, increase/decrease, and ratio calculations |
| 🧾 GST Calculator | Add or remove GST from any amount |
| 📐 Unit Converter | Convert between length, weight, temperature, and more |
| 📷 QR Code Generator | Generate QR codes from any text or URL |
| 💸 Expense Splitter | Split a bill evenly among any number of people |
| 👛 Wallet | Simple personal budget tracker |
| 📊 Analytics | Visual charts for your calculation history |

**Key features:** favorites, dark/light theme, calculation history stored locally with SQLite, Syncfusion Charts and Barcode controls.

---

### 2. Smart Expense Tracker

A full-featured personal finance app for tracking income and expenses.

**Features:**
- 🔐 User authentication (register & login)
- 📊 Dashboard with total balance, income, expenses, and savings summary
- 📈 Monthly trend spline chart and category doughnut chart
- ➕ Add transactions with category, type (income/expense), and date
- 📋 Transactions list with filtering and search
- 📄 Custom reports with PDF export
- 🤖 AI-powered spending summary (Android only, via on-device AI)
- 🌙 Dark / light theme support

---

### 3. Weather Forecast

A beautifully designed weather app with a dark glassmorphism UI, real-time weather data, hourly/daily forecasts, and rich data visualisations.

**Features:**
- 🌡️ **Today tab** — current conditions with large temperature display, weather emoji, condition badge, feels-like, and daily high/low
- 📊 **3 stat cards** — humidity, wind speed, and feels-like temperature at a glance
- 🔍 **City search** — autocomplete search bar to switch locations instantly
- 📈 **Temperature Trend chart** — spline area chart showing the hourly temperature curve for the day
- 🕐 **Next Hours list** — horizontal scrollable hourly forecast cards
- 🌬️ **Atmosphere gauges** — radial gauges for humidity and wind speed
- 🌤️ **Today's Range** — visual progress bar showing the day's temperature range
- 📅 **Next 5 Days tab** — weekly column chart overview plus 5-day forecast rows with condition badges
- 🔄 **Pull-to-refresh** — swipe down to reload weather data
- ✨ **Loading shimmer** — skeleton shimmer while data is fetching
- 🌙 **Dark glassmorphism theme** — deep navy gradient background with frosted-glass cards

**Data source:** [Open-Meteo API](https://open-meteo.com/) (no API key required)

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | .NET 10 MAUI |
| UI Controls | Syncfusion MAUI Controls v32.2.5 |
| MVVM | CommunityToolkit.Mvvm v8.4.0 |
| Local Database | SQLite (sqlite-net-pcl v1.9.172) |
| Target Platforms | Android (API 21+), iOS, macOS Catalyst, Windows (10.0.17763.0+) |

---

## ✅ Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2022 (v17.12+)](https://visualstudio.microsoft.com/) with the **.NET MAUI** workload installed  
  *(or VS Code with the C# Dev Kit and MAUI extensions)*
- Android SDK (for Android targets) — installed automatically via Visual Studio
- A **Syncfusion license key** — a free [Community License](https://www.syncfusion.com/products/communitylicense) is available for individuals and small businesses

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/PaulAndersonS/maui-mini-apps.git
cd maui-mini-apps
```

### 2. Add your Syncfusion license key

Each app reads the license key from `AppConstants.SyncfusionLicenseKey`.

Open the constants file for each project and replace the placeholder with your key:

- **SmartDailyTools:** `SmartDailyTools/SmartDailyTools/Helpers/AppConstants.cs`
- **SmartExpense:** `SmartExpense/Helpers/AppConstants.cs`

```csharp
public static class AppConstants
{
    public const string SyncfusionLicenseKey = "YOUR_LICENSE_KEY_HERE";
}
```

For **WeatherForecast**, register the license key directly in `WeatherForecast/MauiProgram.cs`:

```csharp
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR_LICENSE_KEY_HERE");
```

### 3. Open and run a project

**Smart Daily Tools**
```
Open: SmartDailyTools/SmartDailyTools.sln
```

**Smart Expense Tracker**
```
Open: SmartExpense/SmartExpense.sln
```

**Weather Forecast**
```
Open: WeatherForecast/WeatherForecast.sln
```

Select your target device (Android emulator/device or Windows) and press **F5** to build and run.

---

## 📂 Project Structure

```
maui-mini-apps/
├── SmartDailyTools/
│   ├── SmartDailyTools.sln
│   └── SmartDailyTools/
│       ├── Data/           # SQLite database service
│       ├── Helpers/        # Converters, constants, utilities
│       ├── Models/         # Data models (BmiHistory, EmiHistory, …)
│       ├── Services/       # Business logic (BmiService, EmiService, …)
│       ├── ViewModels/     # MVVM view models
│       └── Views/          # XAML pages (HomePage, BmiPage, QrPage, …)
│
├── SmartExpense/
│   ├── SmartExpense.sln
│   ├── Data/               # SQLite DB context and repositories
│   ├── Helpers/            # Constants, converters, seed data
│   ├── Models/             # Transaction, Category, AppUser, …
│   ├── Services/           # Auth, transaction, report, AI services
│   ├── ViewModels/         # MVVM view models
│   └── Views/              # XAML pages (DashboardPage, ReportsPage, …)
│
└── WeatherForecast/
    ├── WeatherForecast.sln
    └── WeatherForecast/
        ├── Models/         # API response DTOs, HourlyForecast, DailyForecast
        ├── Services/       # WeatherService (Open-Meteo HTTP client)
        ├── ViewModels/     # WeatherViewModel
        └── Views/          # WeatherDashboardPage
```

---

## 📄 License

This project is provided for educational and demonstration purposes.  
Syncfusion controls are subject to [Syncfusion's license terms](https://www.syncfusion.com/eula/es).

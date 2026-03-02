namespace SmartDailyTools.Services;

public interface IUnitConverterService
{
    double Convert(string category, double value, string fromUnit, string toUnit);
    List<string> GetUnitsForCategory(string category);
    List<string> GetCategories();
}

public class UnitConverterService : IUnitConverterService
{
    private static readonly Dictionary<string, List<string>> _units = new()
    {
        ["Length"]      = ["Meter", "Kilometer", "Centimeter", "Millimeter", "Mile", "Yard", "Foot", "Inch"],
        ["Weight"]      = ["Kilogram", "Gram", "Milligram", "Pound", "Ounce", "Ton"],
        ["Temperature"] = ["Celsius", "Fahrenheit", "Kelvin"]
    };

    // Base: Meter for Length, Gram for Weight, Celsius for Temperature
    private static readonly Dictionary<string, double> _toBase = new()
    {
        // Length → meter
        ["Meter"] = 1, ["Kilometer"] = 1000, ["Centimeter"] = 0.01,
        ["Millimeter"] = 0.001, ["Mile"] = 1609.344, ["Yard"] = 0.9144,
        ["Foot"] = 0.3048, ["Inch"] = 0.0254,
        // Weight → gram
        ["Kilogram"] = 1000, ["Gram"] = 1, ["Milligram"] = 0.001,
        ["Pound"] = 453.592, ["Ounce"] = 28.3495, ["Ton"] = 1_000_000
    };

    public List<string> GetCategories() => [.. _units.Keys];

    public List<string> GetUnitsForCategory(string category)
        => _units.TryGetValue(category, out var list) ? list : [];

    public double Convert(string category, double value, string fromUnit, string toUnit)
    {
        if (fromUnit == toUnit) return value;

        if (category == "Temperature")
            return ConvertTemperature(value, fromUnit, toUnit);

        if (!_toBase.TryGetValue(fromUnit, out var fromFactor) ||
            !_toBase.TryGetValue(toUnit, out var toFactor))
            return 0;

        return value * fromFactor / toFactor;
    }

    private static double ConvertTemperature(double value, string from, string to)
    {
        // Normalise to Celsius first
        double celsius = from switch
        {
            "Celsius"    => value,
            "Fahrenheit" => (value - 32) * 5 / 9,
            "Kelvin"     => value - 273.15,
            _            => value
        };

        return to switch
        {
            "Celsius"    => celsius,
            "Fahrenheit" => celsius * 9 / 5 + 32,
            "Kelvin"     => celsius + 273.15,
            _            => celsius
        };
    }
}

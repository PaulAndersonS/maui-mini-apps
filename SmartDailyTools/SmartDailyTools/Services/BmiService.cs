using SmartDailyTools.Helpers;

namespace SmartDailyTools.Services;

public interface IBmiService
{
    double Calculate(double weight, double height, string weightUnit, string heightUnit);
    string GetCategory(double bmi);
    Color GetCategoryColor(string category);
}

public class BmiService : IBmiService
{
    public double Calculate(double weight, double height, string weightUnit, string heightUnit)
    {
        // Normalise to kg / m
        double weightKg = weightUnit.ToLower() switch
        {
            "lb" or "lbs" => weight * 0.453592,
            _ => weight
        };

        double heightM = heightUnit.ToLower() switch
        {
            "cm" => height / 100.0,
            "ft" => height * 0.3048,
            "in" => height * 0.0254,
            _ => height
        };

        if (heightM <= 0) return 0;
        return Math.Round(weightKg / (heightM * heightM), 1);
    }

    public string GetCategory(double bmi)
    {
        return bmi switch
        {
            < 18.5 => AppConstants.BmiUnderweight,
            < 25.0 => AppConstants.BmiNormal,
            < 30.0 => AppConstants.BmiOverweight,
            _ => AppConstants.BmiObese
        };
    }

    public Color GetCategoryColor(string category)
    {
        return category switch
        {
            AppConstants.BmiUnderweight => Color.FromArgb("#3B82F6"),
            AppConstants.BmiNormal => Color.FromArgb("#22C55E"),
            AppConstants.BmiOverweight => Color.FromArgb("#F59E0B"),
            _ => Color.FromArgb("#EF4444")
        };
    }
}

using SQLite;

namespace SmartDailyTools.Models;

[Table("BmiHistory")]
public class BmiHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public double Height { get; set; }
    public string HeightUnit { get; set; } = "cm";
    public double Weight { get; set; }
    public string WeightUnit { get; set; } = "kg";
    public double BmiValue { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Ignore]
    public string FormattedDate => CreatedAt.ToLocalTime().ToString("dd MMM yyyy, hh:mm tt");

    [Ignore]
    public string Summary => $"BMI: {BmiValue:F1} ({Category})";
}

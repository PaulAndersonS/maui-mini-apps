using SQLite;

namespace SmartDailyTools.Models;

[Table("AgeHistory")]
public class AgeHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public DateTime DateOfBirth { get; set; }
    public int Years { get; set; }
    public int Months { get; set; }
    public int Days { get; set; }
    public int TotalDays { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Ignore]
    public string FormattedDate => CreatedAt.ToLocalTime().ToString("dd MMM yyyy, hh:mm tt");

    [Ignore]
    public string Summary => $"{Years}y {Months}m {Days}d | {TotalDays:N0} days";

    [Ignore]
    public string DobFormatted => DateOfBirth.ToString("dd MMM yyyy");
}

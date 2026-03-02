using SQLite;

namespace SmartDailyTools.Models;

[Table("UnitConverterHistory")]
public class UnitConverterHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Category { get; set; } = string.Empty;   // Length | Weight | Temperature
    public double InputValue { get; set; }
    public string FromUnit { get; set; } = string.Empty;
    public double OutputValue { get; set; }
    public string ToUnit { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Ignore]
    public string FormattedDate => CreatedAt.ToLocalTime().ToString("dd MMM yyyy, hh:mm tt");

    [Ignore]
    public string Summary => $"{InputValue} {FromUnit} = {OutputValue:G6} {ToUnit}";
}

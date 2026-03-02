using SQLite;

namespace SmartDailyTools.Models;

[Table("GstHistory")]
public class GstHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public double BaseAmount { get; set; }
    public double GstPercent { get; set; }
    public double GstAmount { get; set; }
    public double TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Ignore]
    public string FormattedDate => CreatedAt.ToLocalTime().ToString("dd MMM yyyy, hh:mm tt");

    [Ignore]
    public string Summary => $"GST {GstPercent}%: ₹{BaseAmount:N2} → ₹{TotalAmount:N2}";
}

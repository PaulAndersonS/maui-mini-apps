using SQLite;

namespace SmartDailyTools.Models;

[Table("ExpenseSplitterHistory")]
public class ExpenseSplitterHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public double TotalBill { get; set; }
    public int NumberOfPeople { get; set; }
    public double TipPercent { get; set; }
    public double PerPersonAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Ignore]
    public string FormattedDate => CreatedAt.ToLocalTime().ToString("dd MMM yyyy, hh:mm tt");

    [Ignore]
    public string Summary => $"₹{TotalBill:N2} ÷ {NumberOfPeople} = ₹{PerPersonAmount:N2}/person";
}

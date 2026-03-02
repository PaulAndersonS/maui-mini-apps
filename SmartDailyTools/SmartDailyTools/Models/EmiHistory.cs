using SQLite;

namespace SmartDailyTools.Models;

[Table("EmiHistory")]
public class EmiHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public double LoanAmount { get; set; }
    public double InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public double Emi { get; set; }
    public double TotalInterest { get; set; }
    public double TotalPayment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Ignore]
    public string FormattedDate => CreatedAt.ToLocalTime().ToString("dd MMM yyyy, hh:mm tt");

    [Ignore]
    public string Summary => $"EMI: ₹{Emi:N2} | ₹{LoanAmount:N0} @ {InterestRate}%";
}

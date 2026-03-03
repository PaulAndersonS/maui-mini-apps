namespace SmartExpense.Models;

public class MonthlyTrendPoint
{
    public string Month { get; set; } = string.Empty;
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
    public decimal Savings => Income - Expense;
}

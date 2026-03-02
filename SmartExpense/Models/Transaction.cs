using SQLite;

namespace SmartExpense.Models;

public class Transaction
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    [MaxLength(64)]
    public string Category { get; set; } = string.Empty;

    public TransactionType Type { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string Notes { get; set; } = string.Empty;
}

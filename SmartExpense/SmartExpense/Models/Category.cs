using SQLite;

namespace SmartExpense.Models;

public class Category
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Icon { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Color { get; set; } = "#4CAF50";
}

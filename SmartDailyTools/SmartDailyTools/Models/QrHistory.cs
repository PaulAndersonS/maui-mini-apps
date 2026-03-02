using SQLite;

namespace SmartDailyTools.Models;

[Table("QrHistory")]
public class QrHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string InputText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Ignore]
    public string FormattedDate => CreatedAt.ToLocalTime().ToString("dd MMM yyyy, hh:mm tt");

    [Ignore]
    public string Summary => InputText.Length > 40 ? InputText[..40] + "..." : InputText;
}

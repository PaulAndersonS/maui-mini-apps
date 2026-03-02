using SQLite;

namespace SmartDailyTools.Models;

[Table("PercentageHistory")]
public class PercentageHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Mode { get; set; } = string.Empty;  // "XPercentOfY" | "WhatPercent" | "PercentChange"
    public double InputA { get; set; }
    public double InputB { get; set; }
    public double Result { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Ignore]
    public string FormattedDate => CreatedAt.ToLocalTime().ToString("dd MMM yyyy, hh:mm tt");

    [Ignore]
    public string Summary => $"{Mode}: {Result:N2}";
}

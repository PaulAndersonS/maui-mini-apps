using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using SmartExpense.Models;

namespace SmartExpense.Services;

public class ReportService : IReportService
{
    public async Task<string> ExportMonthlyReportAsync(
        int year,
        int month,
        DashboardSummary summary,
        List<Transaction> transactions,
        List<CategoryBreakdownItem> categoryBreakdown)
    {
        using var document = new PdfDocument();
        var page = document.Pages.Add();
        var graphics = page.Graphics;

        var headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 18, PdfFontStyle.Bold);
        var normalFont = new PdfStandardFont(PdfFontFamily.Helvetica, 11);
        var boldFont = new PdfStandardFont(PdfFontFamily.Helvetica, 11, PdfFontStyle.Bold);

        var y = 20f;
        var monthName = new DateTime(year, month, 1).ToString("MMMM yyyy");
        graphics.DrawString($"SmartExpense Monthly Report - {monthName}", headerFont, PdfBrushes.Black, 20, y);
        y += 40;

        graphics.DrawString($"Total Income: {summary.CurrentMonthIncome:C2}", boldFont, PdfBrushes.DarkGreen, 20, y);
        y += 20;
        graphics.DrawString($"Total Expense: {summary.CurrentMonthExpense:C2}", boldFont, PdfBrushes.DarkRed, 20, y);
        y += 20;
        graphics.DrawString($"Net Balance: {(summary.CurrentMonthIncome - summary.CurrentMonthExpense):C2}", boldFont, PdfBrushes.Black, 20, y);
        y += 35;

        DrawPieChart(graphics, categoryBreakdown, 20, y, 180, 180);
        y += 200;

        graphics.DrawString("Transactions", boldFont, PdfBrushes.Black, 20, y);
        y += 20;

        DrawTableHeader(graphics, boldFont, y);
        y += 18;

        foreach (var item in transactions)
        {
            if (y > page.GetClientSize().Height - 30)
            {
                page = document.Pages.Add();
                graphics = page.Graphics;
                y = 20;
                DrawTableHeader(graphics, boldFont, y);
                y += 18;
            }

            graphics.DrawString(item.Date.ToString("dd-MMM-yyyy"), normalFont, PdfBrushes.Black, 20, y);
            graphics.DrawString(item.Title, normalFont, PdfBrushes.Black, 110, y);
            graphics.DrawString(item.Category, normalFont, PdfBrushes.Black, 260, y);
            graphics.DrawString(item.Type.ToString(), normalFont, PdfBrushes.Black, 360, y);
            graphics.DrawString(item.Amount.ToString("C2"), normalFont, PdfBrushes.Black, 430, y);
            y += 16;
        }

        var filename = $"SmartExpense_{year}_{month:00}.pdf";
        var path = Path.Combine(FileSystem.CacheDirectory, filename);

        await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        document.Save(stream);
        document.Close(true);

        return path;
    }

    private static void DrawTableHeader(PdfGraphics graphics, PdfFont font, float y)
    {
        graphics.DrawString("Date", font, PdfBrushes.Black, 20, y);
        graphics.DrawString("Title", font, PdfBrushes.Black, 110, y);
        graphics.DrawString("Category", font, PdfBrushes.Black, 260, y);
        graphics.DrawString("Type", font, PdfBrushes.Black, 360, y);
        graphics.DrawString("Amount", font, PdfBrushes.Black, 430, y);
        graphics.DrawLine(PdfPens.Gray, 20, y + 14, 520, y + 14);
    }

    private static void DrawPieChart(PdfGraphics graphics, IReadOnlyList<CategoryBreakdownItem> items, float x, float y, float width, float height)
    {
        if (items.Count == 0)
        {
            graphics.DrawString("No category data for this month.", new PdfStandardFont(PdfFontFamily.Helvetica, 10), PdfBrushes.Gray, x, y + 20);
            return;
        }

        var colors = new[]
        {
            Syncfusion.Drawing.Color.FromArgb(229, 57, 53),
            Syncfusion.Drawing.Color.FromArgb(142, 36, 170),
            Syncfusion.Drawing.Color.FromArgb(57, 73, 171),
            Syncfusion.Drawing.Color.FromArgb(30, 136, 229),
            Syncfusion.Drawing.Color.FromArgb(0, 137, 123),
            Syncfusion.Drawing.Color.FromArgb(124, 179, 66),
            Syncfusion.Drawing.Color.FromArgb(251, 192, 45),
            Syncfusion.Drawing.Color.FromArgb(251, 140, 0)
        };

        var startAngle = 0f;
        for (var i = 0; i < items.Count; i++)
        {
            var sweepAngle = (float)(items[i].Percentage / 100d * 360d);
            var brush = new PdfSolidBrush(colors[i % colors.Length]);
            graphics.DrawPie(brush, x, y, width, height, startAngle, sweepAngle);
            startAngle += sweepAngle;
        }

        var legendY = y;
        var legendFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
        for (var i = 0; i < items.Count; i++)
        {
            var brush = new PdfSolidBrush(colors[i % colors.Length]);
            graphics.DrawRectangle(brush, x + width + 20, legendY, 10, 10);
            graphics.DrawString($"{items[i].Category} ({items[i].Percentage:F1}%)", legendFont, PdfBrushes.Black, x + width + 35, legendY - 1);
            legendY += 14;
        }
    }
}

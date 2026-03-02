using SmartDailyTools.Models;

namespace SmartDailyTools.Data;

public interface IDatabaseService
{
    Task InitializeAsync();

    // BMI
    Task<List<BmiHistory>> GetBmiHistoryAsync();
    Task SaveBmiHistoryAsync(BmiHistory record);
    Task DeleteBmiHistoryAsync(BmiHistory record);

    // Age
    Task<List<AgeHistory>> GetAgeHistoryAsync();
    Task SaveAgeHistoryAsync(AgeHistory record);
    Task DeleteAgeHistoryAsync(AgeHistory record);

    // EMI
    Task<List<EmiHistory>> GetEmiHistoryAsync();
    Task SaveEmiHistoryAsync(EmiHistory record);
    Task DeleteEmiHistoryAsync(EmiHistory record);

    // Percentage
    Task<List<PercentageHistory>> GetPercentageHistoryAsync();
    Task SavePercentageHistoryAsync(PercentageHistory record);
    Task DeletePercentageHistoryAsync(PercentageHistory record);

    // GST
    Task<List<GstHistory>> GetGstHistoryAsync();
    Task SaveGstHistoryAsync(GstHistory record);
    Task DeleteGstHistoryAsync(GstHistory record);

    // Unit Converter
    Task<List<UnitConverterHistory>> GetUnitConverterHistoryAsync();
    Task SaveUnitConverterHistoryAsync(UnitConverterHistory record);
    Task DeleteUnitConverterHistoryAsync(UnitConverterHistory record);

    // QR
    Task<List<QrHistory>> GetQrHistoryAsync();
    Task SaveQrHistoryAsync(QrHistory record);
    Task DeleteQrHistoryAsync(QrHistory record);

    // Expense Splitter
    Task<List<ExpenseSplitterHistory>> GetExpenseSplitterHistoryAsync();
    Task SaveExpenseSplitterHistoryAsync(ExpenseSplitterHistory record);
    Task DeleteExpenseSplitterHistoryAsync(ExpenseSplitterHistory record);

    // Utility
    Task ClearAllHistoryAsync();
}

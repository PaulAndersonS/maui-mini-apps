using SQLite;
using SmartDailyTools.Helpers;
using SmartDailyTools.Models;

namespace SmartDailyTools.Data;

public class DatabaseService : IDatabaseService
{
    private SQLiteAsyncConnection? _db;

    private async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        if (_db is not null)
            return _db;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, AppConstants.DatabaseFileName);
        _db = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
        return _db;
    }

    public async Task InitializeAsync()
    {
        var db = await GetDatabaseAsync();
        await db.CreateTableAsync<BmiHistory>();
        await db.CreateTableAsync<AgeHistory>();
        await db.CreateTableAsync<EmiHistory>();
        await db.CreateTableAsync<PercentageHistory>();
        await db.CreateTableAsync<GstHistory>();
        await db.CreateTableAsync<UnitConverterHistory>();
        await db.CreateTableAsync<QrHistory>();
        await db.CreateTableAsync<ExpenseSplitterHistory>();
    }

    // ── BMI ─────────────────────────────────────────────────────────────────
    public async Task<List<BmiHistory>> GetBmiHistoryAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<BmiHistory>().OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task SaveBmiHistoryAsync(BmiHistory record)
    {
        var db = await GetDatabaseAsync();
        if (record.Id == 0)
            await db.InsertAsync(record);
        else
            await db.UpdateAsync(record);
    }

    public async Task DeleteBmiHistoryAsync(BmiHistory record)
    {
        var db = await GetDatabaseAsync();
        await db.DeleteAsync(record);
    }

    // ── AGE ─────────────────────────────────────────────────────────────────
    public async Task<List<AgeHistory>> GetAgeHistoryAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<AgeHistory>().OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task SaveAgeHistoryAsync(AgeHistory record)
    {
        var db = await GetDatabaseAsync();
        if (record.Id == 0)
            await db.InsertAsync(record);
        else
            await db.UpdateAsync(record);
    }

    public async Task DeleteAgeHistoryAsync(AgeHistory record)
    {
        var db = await GetDatabaseAsync();
        await db.DeleteAsync(record);
    }

    // ── EMI ─────────────────────────────────────────────────────────────────
    public async Task<List<EmiHistory>> GetEmiHistoryAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<EmiHistory>().OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task SaveEmiHistoryAsync(EmiHistory record)
    {
        var db = await GetDatabaseAsync();
        if (record.Id == 0)
            await db.InsertAsync(record);
        else
            await db.UpdateAsync(record);
    }

    public async Task DeleteEmiHistoryAsync(EmiHistory record)
    {
        var db = await GetDatabaseAsync();
        await db.DeleteAsync(record);
    }

    // ── PERCENTAGE ──────────────────────────────────────────────────────────
    public async Task<List<PercentageHistory>> GetPercentageHistoryAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<PercentageHistory>().OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task SavePercentageHistoryAsync(PercentageHistory record)
    {
        var db = await GetDatabaseAsync();
        if (record.Id == 0)
            await db.InsertAsync(record);
        else
            await db.UpdateAsync(record);
    }

    public async Task DeletePercentageHistoryAsync(PercentageHistory record)
    {
        var db = await GetDatabaseAsync();
        await db.DeleteAsync(record);
    }

    // ── GST ─────────────────────────────────────────────────────────────────
    public async Task<List<GstHistory>> GetGstHistoryAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<GstHistory>().OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task SaveGstHistoryAsync(GstHistory record)
    {
        var db = await GetDatabaseAsync();
        if (record.Id == 0)
            await db.InsertAsync(record);
        else
            await db.UpdateAsync(record);
    }

    public async Task DeleteGstHistoryAsync(GstHistory record)
    {
        var db = await GetDatabaseAsync();
        await db.DeleteAsync(record);
    }

    // ── UNIT CONVERTER ──────────────────────────────────────────────────────
    public async Task<List<UnitConverterHistory>> GetUnitConverterHistoryAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<UnitConverterHistory>().OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task SaveUnitConverterHistoryAsync(UnitConverterHistory record)
    {
        var db = await GetDatabaseAsync();
        if (record.Id == 0)
            await db.InsertAsync(record);
        else
            await db.UpdateAsync(record);
    }

    public async Task DeleteUnitConverterHistoryAsync(UnitConverterHistory record)
    {
        var db = await GetDatabaseAsync();
        await db.DeleteAsync(record);
    }

    // ── QR ──────────────────────────────────────────────────────────────────
    public async Task<List<QrHistory>> GetQrHistoryAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<QrHistory>().OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task SaveQrHistoryAsync(QrHistory record)
    {
        var db = await GetDatabaseAsync();
        if (record.Id == 0)
            await db.InsertAsync(record);
        else
            await db.UpdateAsync(record);
    }

    public async Task DeleteQrHistoryAsync(QrHistory record)
    {
        var db = await GetDatabaseAsync();
        await db.DeleteAsync(record);
    }

    // ── EXPENSE SPLITTER ────────────────────────────────────────────────────
    public async Task<List<ExpenseSplitterHistory>> GetExpenseSplitterHistoryAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<ExpenseSplitterHistory>().OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task SaveExpenseSplitterHistoryAsync(ExpenseSplitterHistory record)
    {
        var db = await GetDatabaseAsync();
        if (record.Id == 0)
            await db.InsertAsync(record);
        else
            await db.UpdateAsync(record);
    }

    public async Task DeleteExpenseSplitterHistoryAsync(ExpenseSplitterHistory record)
    {
        var db = await GetDatabaseAsync();
        await db.DeleteAsync(record);
    }

    public async Task ClearAllHistoryAsync()
    {
        var db = await GetDatabaseAsync();
        await db.DeleteAllAsync<BmiHistory>();
        await db.DeleteAllAsync<AgeHistory>();
        await db.DeleteAllAsync<EmiHistory>();
        await db.DeleteAllAsync<PercentageHistory>();
        await db.DeleteAllAsync<GstHistory>();
        await db.DeleteAllAsync<UnitConverterHistory>();
        await db.DeleteAllAsync<QrHistory>();
        await db.DeleteAllAsync<ExpenseSplitterHistory>();
    }
}

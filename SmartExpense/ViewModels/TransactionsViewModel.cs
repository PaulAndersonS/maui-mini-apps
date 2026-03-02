using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SmartExpense.Helpers;
using SmartExpense.Models;
using SmartExpense.Services;
using System.Collections.ObjectModel;

namespace SmartExpense.ViewModels;

public partial class TransactionsViewModel : BaseViewModel
{
    private readonly ITransactionService _transactionService;

    public TransactionsViewModel(ITransactionService transactionService)
    {
        _transactionService = transactionService;
        WeakReferenceMessenger.Default.Register<TransactionsChangedMessage>(this, async (_, _) =>
        {
            await LoadCommand.ExecuteAsync(null);
        });
    }

    public ObservableCollection<Transaction> Transactions { get; } = [];

    [ObservableProperty]
    private DateTime filterMonth = DateTime.Now;

    [ObservableProperty]
    private Transaction? selectedTransaction;

    [ObservableProperty]
    private int selectedIndex = -1;

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var items = await _transactionService.GetTransactionsByMonthAsync(FilterMonth.Year, FilterMonth.Month);
            Transactions.Clear();
            foreach (var item in items)
            {
                Transactions.Add(item);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteSelectedAsync()
    {
        if (SelectedTransaction is null)
        {
            return;
        }

        var page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page is null)
        {
            return;
        }

        var shouldDelete = await page.DisplayAlertAsync("Delete", "Delete selected transaction?", "Yes", "No");
        if (!shouldDelete)
        {
            return;
        }

        await _transactionService.DeleteTransactionAsync(SelectedTransaction);
        WeakReferenceMessenger.Default.Send(new TransactionsChangedMessage(DateTime.UtcNow));
        await LoadAsync();
    }

    [RelayCommand]
    private async Task EditSelectedAsync()
    {
        var page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (SelectedTransaction is null || page is null)
        {
            return;
        }

        var newTitle = await page.DisplayPromptAsync("Edit Title", "Update transaction title", initialValue: SelectedTransaction.Title);
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            return;
        }

        var newAmountText = await page.DisplayPromptAsync("Edit Amount", "Update amount", initialValue: SelectedTransaction.Amount.ToString("F2"), keyboard: Keyboard.Numeric);
        if (!decimal.TryParse(newAmountText, out var newAmount) || newAmount <= 0)
        {
            return;
        }

        SelectedTransaction.Title = newTitle.Trim();
        SelectedTransaction.Amount = newAmount;
        await _transactionService.SaveTransactionAsync(SelectedTransaction);
        WeakReferenceMessenger.Default.Send(new TransactionsChangedMessage(DateTime.UtcNow));
        await LoadAsync();
    }
}

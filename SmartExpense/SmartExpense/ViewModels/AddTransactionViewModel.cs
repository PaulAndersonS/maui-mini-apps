using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SmartExpense.Helpers;
using SmartExpense.Models;
using SmartExpense.Services;
using System.Collections.ObjectModel;

namespace SmartExpense.ViewModels;

public partial class AddTransactionViewModel(ITransactionService transactionService) : BaseViewModel
{
    private readonly ITransactionService _transactionService = transactionService;

    public ObservableCollection<Category> Categories { get; } = [];

    [ObservableProperty]
    private string transactionId = string.Empty;

    [ObservableProperty]
    private string titleText = string.Empty;

    [ObservableProperty]
    private decimal amount;

    [ObservableProperty]
    private Category? selectedCategory;

    [ObservableProperty]
    private DateTime transactionDate = DateTime.Now;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private bool isIncome = false;

    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        if (Categories.Count > 0)
        {
            return;
        }

        Categories.Clear();
        var categories = await _transactionService.GetCategoriesAsync();
        if (categories.Count == 0)
        {
            categories = CategorySeedData.GetDefaultCategories().ToList();
        }

        foreach (var category in categories)
        {
            Categories.Add(category);
        }

        SelectedCategory ??= Categories.FirstOrDefault();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy || string.IsNullOrWhiteSpace(TitleText) || Amount <= 0 || SelectedCategory is null)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var transaction = new Transaction
            {
                Id = string.IsNullOrWhiteSpace(TransactionId) ? Guid.NewGuid().ToString("N") : TransactionId,
                Title = TitleText.Trim(),
                Amount = Amount,
                Category = SelectedCategory.Name,
                Type = IsIncome ? TransactionType.Income : TransactionType.Expense,
                Date = TransactionDate,
                Notes = Notes?.Trim() ?? string.Empty
            };

            await _transactionService.SaveTransactionAsync(transaction);
            WeakReferenceMessenger.Default.Send(new TransactionsChangedMessage(DateTime.UtcNow));
            var page = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (page is not null)
            {
                await page.DisplayAlertAsync("Saved", "Transaction saved successfully.", "OK");
            }

            TitleText = string.Empty;
            Amount = 0;
            Notes = string.Empty;
            TransactionDate = DateTime.Now;
            IsIncome = false;
            TransactionId = string.Empty;
        }
        finally
        {
            IsBusy = false;
        }
    }
}

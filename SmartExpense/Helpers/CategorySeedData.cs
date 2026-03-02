using SmartExpense.Models;

namespace SmartExpense.Helpers;

public static class CategorySeedData
{
    public static IReadOnlyList<Category> GetDefaultCategories() =>
    [
        new Category { Name = "Salary", Icon = "💼", Color = "#2E7D32" },
        new Category { Name = "Freelance", Icon = "🧾", Color = "#00695C" },
        new Category { Name = "Food", Icon = "🍔", Color = "#E64A19" },
        new Category { Name = "Travel", Icon = "✈️", Color = "#1565C0" },
        new Category { Name = "Shopping", Icon = "🛍️", Color = "#6A1B9A" },
        new Category { Name = "Health", Icon = "💊", Color = "#AD1457" },
        new Category { Name = "Utilities", Icon = "💡", Color = "#455A64" },
        new Category { Name = "Other", Icon = "📌", Color = "#5D4037" }
    ];
}

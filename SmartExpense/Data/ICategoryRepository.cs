using SmartExpense.Models;

namespace SmartExpense.Data;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
}

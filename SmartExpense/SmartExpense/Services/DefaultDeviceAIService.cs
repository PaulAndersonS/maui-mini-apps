namespace SmartExpense.Services;

/// <summary>
/// No-op implementation for platforms that don't support native AI assistant invocation.
/// </summary>
public class DefaultDeviceAIService : IDeviceAIService
{
    public Task InvokeAIAssistantAsync(string? context = null) => Task.CompletedTask;
}

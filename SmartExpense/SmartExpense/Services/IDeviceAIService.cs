namespace SmartExpense.Services;

/// <summary>
/// Invokes the device's native AI assistant (e.g. Gemini on Android)
/// with optional financial context pre-filled.
/// </summary>
public interface IDeviceAIService
{
    Task InvokeAIAssistantAsync(string? context = null);
}

using Android.Content;
using SmartExpense.Services;
using AndroidApp = Android.App.Application;

namespace SmartExpense.Platforms.Android;

/// <summary>
/// Invokes the device AI assistant using Intent.ActionAssist.
/// On modern Android (10+) this launches Gemini; on older devices it falls back
/// to Google Assistant. Passes the financial summary as pre-filled context text.
/// </summary>
public class DeviceAIService : IDeviceAIService
{
    public Task InvokeAIAssistantAsync(string? context = null)
    {
        try
        {
            // Primary: invoke whatever assistant is currently set as default (Gemini on modern Android)
            var intent = new Intent(Intent.ActionAssist);
            intent.AddFlags(ActivityFlags.NewTask);

            if (!string.IsNullOrWhiteSpace(context))
            {
                // Gemini / Assistant will receive this as pre-filled input text
                intent.PutExtra(Intent.ExtraText, context);
                intent.PutExtra("android.intent.extra.ASSIST_INPUT", context);
            }

            AndroidApp.Context.StartActivity(intent);
        }
        catch (ActivityNotFoundException)
        {
            // Fallback: launch the Gemini app directly if installed
            try
            {
                var geminiIntent = AndroidApp.Context.PackageManager?
                    .GetLaunchIntentForPackage("com.google.android.apps.bard");

                if (geminiIntent is not null)
                {
                    geminiIntent.AddFlags(ActivityFlags.NewTask);
                    AndroidApp.Context.StartActivity(geminiIntent);
                }
            }
            catch
            {
                // Nothing to do — device has no AI assistant installed
            }
        }

        return Task.CompletedTask;
    }
}

using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SmartExpense.Helpers;

public sealed class TransactionsChangedMessage : ValueChangedMessage<DateTime>
{
    public TransactionsChangedMessage(DateTime value) : base(value)
    {
    }
}

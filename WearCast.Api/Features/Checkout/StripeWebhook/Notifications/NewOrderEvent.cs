using WearCast.Api.Common.Interfaces.Notifications;

namespace WearCast.Api.Features.Checkout.StripeWebhook.Notifications;

public record NewOrderEvent(
    List<string> RecipientIds,
    int OrderId,
    decimal PayoutAmount) : INotification, INotificationEvent
{
    public string Title => "New Order Received";

    public string Message => $"You have received a new order #{OrderId} with payout ${PayoutAmount}";

    public NotificationType NotificationType => NotificationType.NewOrder;

    public int? UrlId => OrderId;
}

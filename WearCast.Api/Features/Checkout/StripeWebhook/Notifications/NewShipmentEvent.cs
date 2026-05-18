using WearCast.Api.Common.Interfaces.Notifications;

namespace WearCast.Api.Features.Checkout.StripeWebhook.Notifications;

public record NewShipmentEvent(
    List<string> RecipientIds,
    int ShipmentId,
    int OrderCount,
    decimal? DeliveryFee = null) : INotification, INotificationEvent
{
    public string Title => "New Shipment Created";

    public string Message => DeliveryFee.HasValue 
        ? $"You have a new shipment #{ShipmentId} with {OrderCount} order(s). Delivery fee: ${DeliveryFee}"
        : $"A new shipment #{ShipmentId} has been created with {OrderCount} order(s)";

    public NotificationType NotificationType => NotificationType.NewShipment;

    public int? UrlId => ShipmentId;
}

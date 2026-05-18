using WearCast.Api.Common.Interfaces.Notifications;

namespace WearCast.Api.Features.Orders.UpdateOrderStatus
{
    public record ShipmentReadyEvent(
        List<string> RecipientIds,
        int ShipmentId) : INotification, INotificationEvent
    {
        public string Title => "Shipment Ready";

        public string Message => $"shipment #{ShipmentId} is now: Ready for delivery";

        public NotificationType NotificationType => NotificationType.ShipmentReady;

        public int? UrlId => ShipmentId;
    }
}

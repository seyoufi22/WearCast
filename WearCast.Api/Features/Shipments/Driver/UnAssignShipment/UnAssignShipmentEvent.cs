using WearCast.Api.Common.Interfaces.Notifications;

namespace WearCast.Api.Features.Shipments.Driver.UnAssignShipment
{
    public record UnAssignShipmentEvent(
        List<string> RecipientIds,
        int ShipmentId) : INotification, INotificationEvent
    {
        public string Title => "Shipment un assigned";

        public string Message => $"Shipment #{ShipmentId} is now: UnAssigned";

        public NotificationType NotificationType => NotificationType.ShipmentUnAssigned;

        public int? UrlId => ShipmentId;
    }
}

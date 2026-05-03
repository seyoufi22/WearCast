using WearCast.Api.Common.Interfaces.Notifications;

namespace WearCast.Api.Features.Shipments.Driver.UpdateShipmentStatus
{
    public record ShipmentUpdateStatusEvent(
        List<string> RecipientIds,
        int ShipmentId,
        string NewStatusName) : INotification, INotificationEvent
    {
        public string Title => "Shipment Update";

        public string Message => $"Your shipment #{ShipmentId} is now: {NewStatusName}";

        public NotificationType NotificationType => NotificationType.ShipmentUpdateStatus;

        public int? UrlId => ShipmentId;
    }
}
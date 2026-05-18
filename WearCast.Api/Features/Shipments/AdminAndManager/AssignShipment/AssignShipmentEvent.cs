using WearCast.Api.Common.Interfaces.Notifications;

namespace WearCast.Api.Features.Shipments.AdminAndManager.AssignShipment
{
    public record AssignShipmentEvent(
            List<string> RecipientIds,
            int ShipmentId,
            string DestinationCity)
            : INotification, INotificationEvent
    {
        public string Title => "New Shipment Assigned";

        public string Message => $"You have been assigned a new shipment #{ShipmentId} to {DestinationCity}. Please check details and start picking up.";

        public NotificationType NotificationType => NotificationType.ShipmentAssigned;

        public int? UrlId => ShipmentId;
    }
}

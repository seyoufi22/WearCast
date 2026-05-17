using WearCast.Api.Common.Interfaces.Notifications;

namespace WearCast.Api.Features.Drivers.DeleteDriver
{
    public record DeleteDriverEvent(
        List<string> RecipientIds,
        string DriverName,
        int AffectedShipmentsCount)
        : INotification, INotificationEvent
    {
        public string Title => "Driver Account Removed";

        public string Message => $"Driver {DriverName} has been deleted from the system. {AffectedShipmentsCount} shipment(s) reverted to unassigned. Please reassign them.";

        public NotificationType NotificationType => NotificationType.DriverDeleted; 

        public int? UrlId => null;
    }
}

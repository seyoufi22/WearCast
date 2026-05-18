using WearCast.Api.Common.Interfaces.Notifications;

namespace WearCast.Api.Features.Drivers.ChangeDriverStatus
{
    public record UpdateDriverStatusEvent(
        List<string> RecipientIds,
        string DriverName,
        int AffectedShipmentsCount) 
        : INotification, INotificationEvent
    {
        public string Title => "Driver Went Offline";

        public string Message => $"Driver {DriverName} became non active. {AffectedShipmentsCount} shipment(s) reverted to unassigned. Please reassign them.";

        public NotificationType NotificationType => NotificationType.DriverDeActivated; 

        public int? UrlId => null; 
    }
}

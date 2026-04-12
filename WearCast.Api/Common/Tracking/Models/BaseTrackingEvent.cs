namespace WearCast.Api.Common.Tracking.Models
{
    public class BaseTrackingEvent
    {
        public string EventType { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}

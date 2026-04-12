namespace WearCast.Api.Common.Tracking.Models
{
    public class FilterEvent : BaseTrackingEvent
    {
        public FilterEvent() { EventType = "filter"; }
        public FilterDetails Filters { get; set; } = new();
    }
}

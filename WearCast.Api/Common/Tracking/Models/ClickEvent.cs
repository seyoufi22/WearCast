namespace WearCast.Api.Common.Tracking.Models
{
    public class ClickEvent : BaseTrackingEvent
    {
        public ClickEvent() { EventType = "click"; }
        public ProductDetails ProductDetails { get; set; } = new();
    }
}

namespace WearCast.Api.Common.Tracking.Models
{
    public class PurchaseEvent : BaseTrackingEvent
    {
        public PurchaseEvent() { EventType = "purchase"; }
        public List<PurchaseProductDetails> Products { get; set; } = new();
    }
}

using WearCast.Api.Common.Tracking.Models;

namespace WearCast.Api.Common.Tracking
{
    public interface ITrackingService
    {
        void TrackClick(ClickEvent clickEvent);
        void TrackFilter(FilterEvent filterEvent);
        void TrackPurchase(PurchaseEvent purchaseEvent);
    }
}

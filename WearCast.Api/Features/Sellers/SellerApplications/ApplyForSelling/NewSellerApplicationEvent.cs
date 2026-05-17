using WearCast.Api.Common.Interfaces.Notifications;

namespace WearCast.Api.Features.Sellers.SellerApplications.ApplyForSelling
{
    public record NewSellerApplicationEvent(
        List<string> RecipientIds,
        int ApplicationId,
        string SellerName) : INotification, INotificationEvent
    {
        public string Title => "New Seller Application";

        public string Message => $"Seller {SellerName} has submitted a new application pending review.";

        public NotificationType NotificationType => NotificationType.NewSellerApplication;

        public int? UrlId => ApplicationId;
    }
}

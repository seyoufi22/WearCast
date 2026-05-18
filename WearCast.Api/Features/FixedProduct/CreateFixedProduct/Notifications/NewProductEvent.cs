using WearCast.Api.Common.Interfaces.Notifications;

namespace WearCast.Api.Features.FixedProduct.CreateFixedProduct.Notifications;

public record NewProductEvent(
    List<string> RecipientIds,
    int ProductId,
    string ProductName,
    string ProductType) : INotification, INotificationEvent
{
    public string Title => "New Product Created";

    public string Message => $"A new {ProductType} '{ProductName}' has been created";

    public NotificationType NotificationType => NotificationType.NewProduct;

    public int? UrlId => ProductId;
}

namespace WearCast.Api.Features.NotificationManagement
{
    public static class NotificationErrors
    {
        public static readonly Error NotFound =
            new("Notification.NotFound",
                "No notification found with This ID.",
                StatusCodes.Status404NotFound);
    }
}

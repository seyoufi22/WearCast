namespace WearCast.Api.Features.NotificationManagement
{
    public static class NotificationErrors
    {
        public static readonly Error NotificationNotFound =
            new("Notification.NotificationNotFound",
                "No notification found with This ID.",
                StatusCodes.Status404NotFound);

        public static readonly Error UserNotFound =
            new("Notification.UserNotFound",
                "Invalid user id.",
                StatusCodes.Status404NotFound);
    }
}


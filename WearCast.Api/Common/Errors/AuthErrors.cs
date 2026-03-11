namespace WearCast.Api.Common.Errors
{
    public static class AuthErrors
    {
        public static readonly Error Forbidden = new(
                    "Auth.Forbidden",
                    "You do not have permission to perform this action.",
                    StatusCodes.Status403Forbidden);

        public static readonly Error NoAssociatedFactory = new(
            "Auth.NoAssociatedFactory",
            "Your account is not associated with any factory.",
            StatusCodes.Status403Forbidden);
    }
}

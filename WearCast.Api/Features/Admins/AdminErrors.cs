namespace WearCast.Api.Features.Admins
{
    public static class AdminErrors
    {
        public static readonly Error NotFound =
            new("Admin.NotFound",
                "No admin found with This ID.",
                StatusCodes.Status404NotFound);

        public static readonly Error NotAnAdmin =
            new("Admin.NotAnAdmin",
                "This id is not for an admin.",
                StatusCodes.Status409Conflict);

        public static readonly Error CannotDeleteSuperAdmin =
            new("Admin.CannotDeleteSuperAdmin",
                "You cannot delete a super admin.",
                StatusCodes.Status409Conflict);

        public static readonly Error AlreadyDeleted =
            new("Admin.AlreadyDeleted",
                "This admin is already deleted.",
                StatusCodes.Status409Conflict);
    }
}

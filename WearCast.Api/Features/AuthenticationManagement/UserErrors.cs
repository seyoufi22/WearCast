namespace WearCast.Api.Features.AuthenticationManagement
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials =
            new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);

        public static readonly Error AccountDeleted =
            new("User.AccountDeleted", "An account with this email was previously deleted.", StatusCodes.Status409Conflict);

        public static readonly Error DisabledUser =
            new("User.DisabledUser", "Disabled user, please contact your administrator", StatusCodes.Status401Unauthorized);

        public static readonly Error LockedUser =
            new("User.LockedUser", "Locked user, please contact your administrator", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidJwtToken =
            new("User.InvalidJwtToken", "Invalid jwt token", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidRefreshToken =
            new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

        public static readonly Error DublicatedEmail =
            new("User.DublicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);

        public static readonly Error DublicatedPhoneNumber =
            new("User.DublicatedPhoneNumber", "Another user with the same phone number is already exists", StatusCodes.Status409Conflict);

        public static readonly Error EmailNotConfirmed =
            new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidCode =
            new("User.InvalidCode", "Invalid code", StatusCodes.Status400BadRequest);

        public static readonly Error DublicatedConfirmation =
            new("User.DublicatedConfirmation", "Email already confirmed", StatusCodes.Status400BadRequest);
    }
}

namespace WearCast.Api.Features.Drivers
{
    public static class DriverErrors
    {
        public static readonly Error DublicatedNationalId =
            new("Driver.DublicatedNationalId", "Another driver with the same national id is already exists", StatusCodes.Status409Conflict);
    }
}

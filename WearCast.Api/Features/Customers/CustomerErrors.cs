namespace WearCast.Api.Features.Customers
{
    public static class CustomerErrors
    {
        public static readonly Error CustomerNotFound
            = new("CustomerNotFound", "Customer does not exist.", StatusCodes.Status404NotFound);
    }
}

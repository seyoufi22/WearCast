namespace WearCast.Api.Features.Factories
{
    public static class FactoryErrors
    {
        public static readonly Error FactoryNotFound =
            new("Factory.NotFound", "The specified factory does not exist.", StatusCodes.Status404NotFound);
    }
}

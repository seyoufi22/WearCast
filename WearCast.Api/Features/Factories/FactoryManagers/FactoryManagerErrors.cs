namespace WearCast.Api.Features.Factories.FactoryManagers
{
    public static class FactoryManagerErrors
    {
        public static readonly Error NotFound = new("FactoryManager.NotFound", "The specified factory manager was not found.", StatusCodes.Status404NotFound);
    }
}

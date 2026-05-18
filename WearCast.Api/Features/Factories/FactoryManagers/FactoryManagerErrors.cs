namespace WearCast.Api.Features.Factories.FactoryManagers
{
    public static class FactoryManagerErrors
    {
        public static readonly Error NotFound = 
            new("FactoryManager.NotFound", 
            "The specified factory manager was not found.",
            StatusCodes.Status404NotFound);
        
        public static readonly Error CannotDeleteYourself =
            new("FactoryManager.CannotDeleteYourself",
            "You cannot delete your own manager account.",
            StatusCodes.Status400BadRequest);

        public static readonly Error CurrentManagerNotFound =
            new("FactoryManager.CurrentManagerNotFound",
            "The current user's factory profile could not be found.",
            StatusCodes.Status404NotFound);

        public static readonly Error UnauthorizedToDeleteManager =
            new("FactoryManager.Unauthorized",
            "You are not authorized to delete a manager from a different factory.",
            StatusCodes.Status403Forbidden);

        public static readonly Error CannotDeleteLastManager =
            new("FactoryManager.CannotDeleteLastManager",
            "Cannot delete the last manager of this factory. A factory must have at least one active manager.",
            StatusCodes.Status400BadRequest);
    }
}

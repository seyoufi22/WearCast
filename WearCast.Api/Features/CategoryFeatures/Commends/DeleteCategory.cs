namespace WearCast.Api.Features.CategoryFeatures.Commends
{
    public class DeleteCategory
    {
        public record DeleteCategoryCommand(int Id) : IRequest<bool>;

       
    }

}

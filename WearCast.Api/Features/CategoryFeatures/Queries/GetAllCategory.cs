namespace WearCast.Api.Features.CategoryFeatures.Queries
{
    public class GetAllCategory
    {
        public record CategoryResponse(int Id, string Name);
        public record GetCategoriesQuery : IRequest<List<CategoryResponse>>;
        
    }
}

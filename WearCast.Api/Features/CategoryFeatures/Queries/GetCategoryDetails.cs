namespace WearCast.Api.Features.CategoryFeatures.Queries
{
    public class GetCategoryDetails
    {
        public record CategoryResponse(string Name,string ImageUrl);
        public record GetCategoryByIdQuery(int Id) : IRequest<CategoryResponse>;
        
    }
}

using WearCast.Api.Common.Repository;

namespace WearCast.Api.Features.CategoryFeatures.Queries
{
    public class GetCategoryDetails
    {
        public record CategoryResponse(string Name,string ImageUrl);
        public record GetCategoryByIdQuery(int Id) : IRequest<CategoryResponse>;
        public class GetCategoriesHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
        {
            private readonly IRepository<Category> _categoryRepo;

            public GetCategoriesHandler(IRepository<Category> categoryRepo)
            {
                _categoryRepo = categoryRepo;
            }

            public async Task<CategoryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
            {

                var category = await _categoryRepo.GetAsync(c=>c.Id==request.Id);
                if(category == null)
                {
                    return null;
                }

                return new CategoryResponse(category.Name, category.ImageUrl);
            }
        }
    }
}

using WearCast.Api.Common.Repository;
using static WearCast.Api.Features.CategoryFeatures.Queries.GetAllCategory;

namespace WearCast.Api.Features.CategoryFeatures.Handler
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryResponse>>
    {
        private readonly IRepository<Category> _categoryRepo;

        public GetCategoriesHandler(IRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<List<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {

            var categories = await _categoryRepo.GetAllAsync();

            return categories.Select(c => new CategoryResponse(c.Id, c.Name)).ToList();
        }
    }
}

using WearCast.Api.Common.Repository;
using static WearCast.Api.Features.CategoryFeatures.Queries.GetCategoryDetails;

namespace WearCast.Api.Features.CategoryFeatures.Handler
{
    public class GetCategoryDetailsHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
    {
        private readonly IRepository<Category> _categoryRepo;

        public GetCategoryDetailsHandler(IRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<CategoryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {

            var category = await _categoryRepo.GetAsync(c => c.Id == request.Id);
            if (category == null)
            {
                return null;
            }

            return new CategoryResponse(category.Name, category.ImageUrl);
        }
    }
}

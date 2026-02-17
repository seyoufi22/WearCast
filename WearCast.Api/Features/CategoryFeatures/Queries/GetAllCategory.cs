using WearCast.Api.Common.Repository;

namespace WearCast.Api.Features.CategoryFeatures.Queries
{
    public class GetAllCategory
    {
        public record CategoryResponse(int Id, string Name);
        public record GetCategoriesQuery : IRequest<List<CategoryResponse>>;
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
}

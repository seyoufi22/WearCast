namespace WearCast.Api.Features.CategoryFeatures.GetAllCategory;
public class GetAllCategoryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    private readonly IRepository<Category> _categoryRepo;

    public GetAllCategoryHandler(IRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<List<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepo.GetAllAsync();

        return categories
            .Select(c => new CategoryResponse(c.Id, c.Name))
            .ToList();
    }
}
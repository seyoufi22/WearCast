using WearCast.Api.Features.CategoryFeatures.GetCategoryById.DTOs;

namespace WearCast.Api.Features.CategoryFeatures.GetCategoryById;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdRequestDto, CategoryResponse>
{
    private readonly IRepository<Category> _categoryRepo;

    public GetCategoryByIdHandler(IRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<CategoryResponse> Handle(GetCategoryByIdRequestDto request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepo.GetAsync(c => c.Id == request.Id);

        if (category == null)
            return null;

        return new CategoryResponse(category.Name, category.ImageUrl);
    }
}
using WearCast.Api.Features.CategoryFeatures.GetAllCategory.DTOs;

namespace WearCast.Api.Features.CategoryFeatures.GetAllCategory;
public class GetAllCategoryHandler : IRequestHandler<GetAllCategoryRequestDto, List<GetAllCategoryResponseDto>>
{
    private readonly IRepository<Category> _categoryRepo;

    public GetAllCategoryHandler(IRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<List<GetAllCategoryResponseDto>> Handle(GetAllCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepo.GetAllAsync();

        return categories
            .Select(c => new GetAllCategoryResponseDto(c.Id, c.Name))
            .ToList();
    }
}
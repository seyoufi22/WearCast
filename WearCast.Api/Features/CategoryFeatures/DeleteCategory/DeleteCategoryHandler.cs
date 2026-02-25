using WearCast.Api.Features.CategoryFeatures.DeleteCategory.DTOs;

namespace WearCast.Api.Features.CategoryFeatures.DeleteCategory;

public class DeleteCategoryHandler(IRepository<Category> categoryRepo)
    : IRequestHandler<DeleteCategoryRequestDto, bool>
{
    public async Task<bool> Handle(DeleteCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var category = await categoryRepo.GetAsync(c => c.Id == request.Id);
        if (category == null)
            return false;

        await categoryRepo.SoftDeleteAsync(request.Id);
        return true;
    }
}
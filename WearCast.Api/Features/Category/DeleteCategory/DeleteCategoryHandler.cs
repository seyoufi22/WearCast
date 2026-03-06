using WearCast.Api.Features.Category.DeleteCategory.DTOs;

namespace WearCast.Api.Features.Category.DeleteCategory;

public class DeleteCategoryHandler(IRepository<Entities.Category> categoryRepo)
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
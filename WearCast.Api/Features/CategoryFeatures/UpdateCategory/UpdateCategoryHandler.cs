using WearCast.Api.Features.CategoryFeatures.UpdateCategory.DTOs;

namespace WearCast.Api.Features.CategoryFeatures.UpdateCategory;

public class UpdateCategoryHandler(IRepository<Category> categoryRepo, ImageService imageService)
    : IRequestHandler<UpdateCategoryRequestDto, bool>
{
    public async Task<bool> Handle(UpdateCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var category = await categoryRepo.GetAsync(c => c.Id == request.Id);
        if (category == null)
            return false;

        category.Name = request.Name;

        if (request.Image != null)
        {
            string url = await imageService.UploadAsync(request.Image);
            await imageService.DeleteAsync(category.ImageUrl);
            category.ImageUrl = url;
        }

        await categoryRepo.UpdateAsync(category);
        return true;
    }
}

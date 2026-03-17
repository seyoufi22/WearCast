using WearCast.Api.Features.Category.UpdateCategory.DTOs;

namespace WearCast.Api.Features.Category.UpdateCategory;

public class UpdateCategoryHandler(IRepository<Entities.Category> categoryRepo, ImageService _imageService)
    : IRequestHandler<UpdateCategoryRequestDto, Result>
{
    public async Task<Result> Handle(UpdateCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var category = await categoryRepo.GetAsync(c => c.Id == request.Id);
        if (category == null)
            return Result.Failure(new Error("Category.NotFound", $"Category with ID {request.Id} was not found.", 404));

        string trimmedName = request.Name.Trim();
        var existing = await categoryRepo.GetAsync(c => c.Name.ToLower() == trimmedName.ToLower() && c.Id != request.Id);

        if (existing != null)
        {
            return Result.Failure(
                new Error("Category.DuplicateName", "This category name already exists.", 400));
        }

        if (request.Image != null)
        {
            var imageValidation = _imageService.Validate(request.Image!);
            if (!imageValidation.IsValid)
            {
                return Result.Failure(
                    new Error("Category.InvalidImage", imageValidation.ErrorMessage, 400));
            }
        }

        category.Name = trimmedName;

        if (request.Image != null)
        {
            string? url = await _imageService.UploadAsync(request.Image);
            if (string.IsNullOrEmpty(url))
            {
                return Result.Failure(new Error("Image.UploadFailed", "An error occurred while uploading the image. Please try again.", 400));
            }

            await _imageService.DeleteAsync(category.ImageUrl);
            category.ImageUrl = url;
        }

        await categoryRepo.UpdateAsync(category);
        return Result.Success();
    }
}

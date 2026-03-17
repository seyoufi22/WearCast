using WearCast.Api.Features.Category.CreateCategory.DTOs;

public class CreateCategoryHandler(IRepository<Category> categoryRepo, ImageService _imageService)
    : IRequestHandler<CreateCategoryRequestDto, Result>
{
    public async Task<Result> Handle(CreateCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var imageValidation = _imageService.Validate(request.Image!);
        if (!imageValidation.IsValid)
        {
            return Result.Failure(
                new Error("Category.InvalidImage", imageValidation.ErrorMessage, 400));
        }
        string trimmedName = request.Name.Trim();
        var existing = await categoryRepo.GetAsync(c => c.Name.ToLower() == trimmedName.ToLower());

        if (existing != null && !existing.IsDeleted)
        {
            return Result.Failure(
                new Error("Category.DuplicateName", "This category name already exists.", 400));
        }

        string? url = await _imageService.UploadAsync(request.Image!);

        if (string.IsNullOrEmpty(url))
        {
            return Result.Failure(new Error("Image.UploadFailed", "An error occurred while uploading the image. Please try again.", 400));
        }
        if (existing != null)
        {
            await _imageService.DeleteAsync(existing.ImageUrl);
            existing.IsDeleted = false;
            existing.ImageUrl = url;
            await categoryRepo.UpdateAsync(existing);
            return Result.Success();
        }
        var category = new Category
        {
            Name = request.Name.Trim(),
            ImageUrl = url
        };

        await categoryRepo.CreateAsync(category);
        return Result.Success();
    }
}


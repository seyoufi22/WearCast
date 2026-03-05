using WearCast.Api.Features.Category.CreateCategory.DTOs;

public class CreateCategoryHandler(IRepository<Category> categoryRepo, ImageService imageService)
    : IRequestHandler<CreateCategoryRequestDto, CreateCategoryResponseDto>
{
    public async Task<CreateCategoryResponseDto> Handle(CreateCategoryRequestDto request, CancellationToken cancellationToken)
    {

        string url = await imageService.UploadAsync(request.Image!);

        var existing = await categoryRepo.GetAsync(c => c.Name.ToLower() == request.Name.ToLower());
        if (existing != null)
        {
            await imageService.DeleteAsync(existing.ImageUrl);
            existing.IsDeleted = false;
            existing.ImageUrl = url;
            await categoryRepo.UpdateAsync(existing);
            return new CreateCategoryResponseDto(existing.Id, existing.Name, existing.ImageUrl);
        }
        var category = new Category
        {
            Name = request.Name,
            ImageUrl = url
        };

        await categoryRepo.CreateAsync(category);
        return new CreateCategoryResponseDto(category.Id, category.Name, category.ImageUrl);
    }
}


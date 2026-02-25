namespace WearCast.Api.Features.CategoryFeatures.CreateCategory.DTOs;

public record CreateCategoryRequestDto(string Name, IFormFile? Image) : IRequest<CategoryDto>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryRequestDto>
{
    private readonly IRepository<Category> _categoryRepo;
    private readonly ImageService _imageService;

    public CreateCategoryCommandValidator(IRepository<Category> categoryRepo, ImageService imageService)
    {
        _categoryRepo = categoryRepo;
        _imageService = imageService;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MustAsync(BeUniqueName).WithMessage("Category name already exists.");

        RuleFor(x => x.Image)
            .NotNull().WithMessage("Image is required.")
            .Must((command, file, context) =>
            {
                var result = _imageService.Validate(file!);
                if (!result.IsValid)
                    context.AddFailure(result.ErrorMessage);
                return true;
            });
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken token)
    {
        var existing = await _categoryRepo.GetAsync(c => c.Name.ToLower() == name.ToLower() && !c.IsDeleted);
        return existing == null;
    }
}
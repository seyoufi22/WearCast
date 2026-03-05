namespace WearCast.Api.Features.Category.UpdateCategory.DTOs;

public record UpdateCategoryRequestDto(int Id, string Name, IFormFile? Image) : IRequest<bool>;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryRequestDto>
{
    private readonly IRepository<Entities.Category> _categoryRepo;
    private readonly ImageService _imageService;

    public UpdateCategoryCommandValidator(IRepository<Entities.Category> categoryRepo, ImageService imageService)
    {
        _categoryRepo = categoryRepo;
        _imageService = imageService;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid category ID.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MustAsync(BeUniqueName).WithMessage("Category name already exists.");

        RuleFor(x => x.Image)
            .Must((command, file, context) =>
            {
                if (file == null) return true;
                var result = _imageService.Validate(file);
                if (!result.IsValid)
                    context.AddFailure(result.ErrorMessage);
                return true;
            });
    }

    private async Task<bool> BeUniqueName(UpdateCategoryRequestDto command, string name, CancellationToken token)
    {
        var existing = await _categoryRepo.GetAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != command.Id);
        return existing == null;
    }
}
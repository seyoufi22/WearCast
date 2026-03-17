namespace WearCast.Api.Features.Category.CreateCategory.DTOs;

public record CreateCategoryRequestDto(string Name, IFormFile Image) : IRequest<Result>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryRequestDto>
{

    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.");

        RuleFor(x => x.Image)
            .NotNull().WithMessage("Image is required.");
    }
}
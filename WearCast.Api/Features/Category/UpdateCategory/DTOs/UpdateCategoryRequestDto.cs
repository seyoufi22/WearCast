namespace WearCast.Api.Features.Category.UpdateCategory.DTOs;

public record UpdateCategoryRequestDto(int Id, string Name, IFormFile? Image) : IRequest<Result>;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryRequestDto>
{
    public UpdateCategoryCommandValidator()
    {

        RuleFor(x => x.Id)
            .NotEmpty().GreaterThan(0).WithMessage("Valid Category ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.");
    }
}
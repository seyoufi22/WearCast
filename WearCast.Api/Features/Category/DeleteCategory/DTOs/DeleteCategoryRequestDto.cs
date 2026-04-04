namespace WearCast.Api.Features.Category.DeleteCategory.DTOs;

public record DeleteCategoryRequestDto(int Id) : IRequest<Result>;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryRequestDto>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid category ID.");
    }
}
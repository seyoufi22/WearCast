namespace WearCast.Api.Features.CategoryFeatures.DeleteCategory;

public record DeleteCategoryCommand(int Id) : IRequest<bool>;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid category ID.");
    }
}
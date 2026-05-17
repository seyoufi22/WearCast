namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetAllFactoryProductsForFactory
{
    public class GetAllFactoryProductsForFactoryRequestValidator : AbstractValidator<GetAllFactoryProductsForFactoryRequest>
    {
        public GetAllFactoryProductsForFactoryRequestValidator()
        {
            RuleFor(x => x.PageIndex)
                            .GreaterThan(0)
                            .WithMessage("Page index must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.SearchTerm)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm))
                .WithMessage("Search term cannot exceed 100 characters.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .When(x => x.CategoryId.HasValue)
                .WithMessage("Category ID must be greater than 0.");

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinPrice.HasValue)
                .WithMessage("Min price cannot be negative.");

            RuleFor(x => x.MaxPrice)
                .GreaterThan(x => x.MinPrice ?? 0)
                .When(x => x.MaxPrice.HasValue)
                .WithMessage("Max price must be greater than min price.");

            RuleFor(x => x.DressStyle)
                .IsInEnum()
                .When(x => x.DressStyle.HasValue)
                .WithMessage("Invalid dress style.");

            RuleFor(x => x.TargetAudiences)
                .IsInEnum()
                .When(x => x.TargetAudiences.HasValue)
                .WithMessage("Invalid target audience.");

            RuleFor(x => x.SortBy)
                .IsInEnum()
                .WithMessage("Invalid sort option.");
        }
    }
}

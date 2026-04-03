using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProducts.DTOs;

public record GetAllFixedProductsQuery(
    int PageIndex = 1,
    int PageSize = 100,
    string? Category = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    TargetAudience? TargetAudience = null,
    List<Size>? Sizes = null,
    SortBy SortBy = SortBy.Newest
) : IRequest<Result<PagingViewModel<GetAllFixedProductsResponseDto>>>;
public class GetAllFixedProductsQueryValidator : AbstractValidator<GetAllFixedProductsQuery>
{
    public GetAllFixedProductsQueryValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0)
            .WithMessage("Page index must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinPrice.HasValue)
            .WithMessage("Min price cannot be negative.");

        RuleFor(x => x.MaxPrice)
            .GreaterThan(x => x.MinPrice ?? 0)
            .When(x => x.MaxPrice.HasValue)
            .WithMessage("Max price must be greater than min price.");
    }
}

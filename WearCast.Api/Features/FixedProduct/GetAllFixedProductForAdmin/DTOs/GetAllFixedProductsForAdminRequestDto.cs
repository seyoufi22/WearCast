using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsForAdmin.DTOs;

public record GetAllFixedProductsForAdminRequestDto : IRequest<Result<PagingViewModel<GetAllFixedProductsForAdminResponseDto>>>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 30;
    public string? SearchTerm { get; set; } = null;
    public int? CategoryId { get; set; } = null;
    public decimal? MinPrice { get; set; } = null;
    public decimal? MaxPrice { get; set; } = null;
    public DressStyle? DressStyle { get; set; } = null;
    public TargetAudience? TargetAudience { get; set; } = null;
    public List<Size>? Sizes { get; set; } = null;
    public SortBy SortBy { get; set; } = SortBy.Newest;

}
public class GetAllFixedProductsForAdminRequestValidator : AbstractValidator<GetAllFixedProductsForAdminRequestDto>
{
    public GetAllFixedProductsForAdminRequestValidator()
    {
        RuleFor(x => x.PageIndex).GreaterThan(0).WithMessage("Page index must be greater than 0.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue).WithMessage("Min price cannot be negative.");
        RuleFor(x => x.MaxPrice).GreaterThan(x => x.MinPrice ?? 0).When(x => x.MaxPrice.HasValue).WithMessage("Max price must be greater than min price.");
        RuleFor(x => x.TargetAudience).IsInEnum().When(x => x.TargetAudience.HasValue).WithMessage("Invalid target audience.");
        RuleFor(x => x.SortBy).IsInEnum().WithMessage("Invalid sort option.");
        RuleForEach(x => x.Sizes).IsInEnum().WithMessage("Invalid size value.");
    }
}
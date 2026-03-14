using WearCast.Api.Abstractions;
using FluentValidation;
using WearCast.Api.Common.Views;
using MediatR;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProducts.DTOs;

public record GetAllFixedProductsQuery(int PageIndex = 1, int PageSize = 100) : IRequest<Result<PagingViewModel<GetAllFixedProductsResponseDto>>>;

public class GetAllFixedProductsQueryValidator : AbstractValidator<GetAllFixedProductsQuery>
{
    public GetAllFixedProductsQueryValidator()
    {
        RuleFor(x => x.PageIndex).GreaterThan(0).WithMessage("PageIndex must be greater than 0.");
        RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
    }
}

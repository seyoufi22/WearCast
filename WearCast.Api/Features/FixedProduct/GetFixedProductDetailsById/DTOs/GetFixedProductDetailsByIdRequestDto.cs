using WearCast.Api.Abstractions;
using FluentValidation;

namespace WearCast.Api.Features.FixedProduct.GetFixedProductDetailsById.DTOs;

public record GetFixedProductDetailsByIdQuery(int Id) : IRequest<Result<GetFixedProductDetailsByIdResponseDto>>;

public class GetFixedProductDetailsByIdQueryValidator : AbstractValidator<GetFixedProductDetailsByIdQuery>
{
    public GetFixedProductDetailsByIdQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
    }
}

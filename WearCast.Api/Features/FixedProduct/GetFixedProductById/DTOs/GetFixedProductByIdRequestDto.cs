using WearCast.Api.Abstractions;
using FluentValidation;

namespace WearCast.Api.Features.FixedProduct.GetFixedProductById.DTOs;

public record GetFixedProductByIdQuery(int Id) : IRequest<Result<GetFixedProductByIdResponseDto>>;

public class GetFixedProductByIdQueryValidator : AbstractValidator<GetFixedProductByIdQuery>
{
    public GetFixedProductByIdQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
    }
}

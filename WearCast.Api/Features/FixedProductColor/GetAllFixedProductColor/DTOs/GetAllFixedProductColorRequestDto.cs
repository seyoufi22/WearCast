namespace WearCast.Api.Features.FixedProductColor.GetAllFixedProductColor.DTOs;

public record GetAllFixedProductColorRequestDto(int ProductId) : IRequest<List<GetAllFixedProductColorResponseDto>>;
public class GetAllFixedProductColorValidator : AbstractValidator<GetAllFixedProductColorRequestDto>
{
    public GetAllFixedProductColorValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.")
            .GreaterThan(0).WithMessage("Product ID must be greater than zero.");
    }
}
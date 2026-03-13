namespace WearCast.Api.Features.FixedProductColor.GetFixedProductColorById.DTOs;
public record GetFixedProductColorByIdRequestDto(int ColorId) : IRequest<GetFixedProductColorByIdResponseDto>;
public class GetFixedProductColorByIdValidator : AbstractValidator<GetFixedProductColorByIdRequestDto>
{
    public GetFixedProductColorByIdValidator()
    {
        RuleFor(x => x.ColorId)
            .GreaterThan(0).WithMessage("ColorId must be greater than 0");
    }
}
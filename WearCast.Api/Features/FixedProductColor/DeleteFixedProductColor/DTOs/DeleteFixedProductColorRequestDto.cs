namespace WearCast.Api.Features.FixedProductColor.DeleteFixedProductColor.DTOs;

public record DeleteFixedProductColorRequestDto(int ColorId, int sellerId, bool isAdminRequest) : IRequest<Result>;

public class DeleteFixedProductColorValidator : AbstractValidator<DeleteFixedProductColorRequestDto>
{
    public DeleteFixedProductColorValidator()
    {
        RuleFor(x => x.ColorId)
            .GreaterThan(0).WithMessage("ColorId must be greater than 0.");
    }
}
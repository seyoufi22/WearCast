namespace WearCast.Api.Features.FixedProductColor.DeleteFixedProductColor.DTOs;

public class DeleteFixedProductColorRequestDto : IRequest<bool>
{
    public int ColorId { get; set; }
}

public class DeleteFixedProductColorValidator : AbstractValidator<DeleteFixedProductColorRequestDto>
{
    public DeleteFixedProductColorValidator()
    {
        RuleFor(x => x.ColorId)
            .GreaterThan(0).WithMessage("ColorId must be greater than 0.");
    }
}
namespace WearCast.Api.Features.FixedProductColor.DeleteFixedProductImage.DTOs;

public class DeleteFixedProductImageRequestDto : IRequest<bool>
{
    public int ImageId { get; set; }
}

public class DeleteFixedProductImageValidator : AbstractValidator<DeleteFixedProductImageRequestDto>
{
    public DeleteFixedProductImageValidator()
    {
        RuleFor(x => x.ImageId)
            .GreaterThan(0).WithMessage("ImageId must be greater than 0.");
    }
}
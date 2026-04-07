namespace WearCast.Api.Features.Sellers.UpdateSellerImage
{
    public class UpdateSellerImageRequestValidator : AbstractValidator<UpdateSellerImageRequest>
    {
        public UpdateSellerImageRequestValidator()
        {
            RuleFor(x => x.NewLogo)
                .NotNull()
                .IsValidImage();
        }
    }
}

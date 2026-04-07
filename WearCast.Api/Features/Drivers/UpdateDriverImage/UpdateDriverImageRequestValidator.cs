namespace WearCast.Api.Features.Drivers.UpdateDriverImage
{
    public class UpdateDriverImageRequestValidator : AbstractValidator<UpdateDriverImageRequest>
    {
        public UpdateDriverImageRequestValidator()
        {
            RuleFor(x => x.NewImage)
                .NotNull()
                .IsValidImage();
        }
    }
}

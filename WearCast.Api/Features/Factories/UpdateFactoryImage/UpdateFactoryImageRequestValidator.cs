namespace WearCast.Api.Features.Factories.UpdateFactoryImage
{
    public class UpdateFactoryImageRequestValidator : AbstractValidator<UpdateFactoryImageRequest>
    {
        public UpdateFactoryImageRequestValidator()
        {
            RuleFor(x => x.NewLogo)
                .NotNull()
                .IsValidImage();
        }
    }
}

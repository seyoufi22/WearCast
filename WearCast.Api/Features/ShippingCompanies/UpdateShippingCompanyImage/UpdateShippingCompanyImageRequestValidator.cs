namespace WearCast.Api.Features.ShippingCompanies.UpdateShippingCompanyImage
{
    public class UpdateShippingCompanyImageRequestValidator : AbstractValidator<UpdateShippingCompanyImageRequest>
    {
        public UpdateShippingCompanyImageRequestValidator()
        {
            RuleFor(x => x.NewLogo)
               .NotNull()
               .IsValidImage();
        }
    }
}

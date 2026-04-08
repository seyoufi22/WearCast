namespace WearCast.Api.Features.Customers.CutomerImage.UpdateCusomerImage
{
    public class UpdateCustomerImageRequestValidator : AbstractValidator<UpdateCustomerImageRequest>
    {
        public UpdateCustomerImageRequestValidator()
        {
            RuleFor(x => x.NewImage)
                .NotNull()
                .IsValidImage();
        }
    }
}

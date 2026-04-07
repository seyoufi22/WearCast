namespace WearCast.Api.Features.Sellers.SellerManagers.UpdateSellerManager
{
    public class UpdateSellerManagerRequestValidator : AbstractValidator<UpdateSellerManagerRequest>
    {
        public UpdateSellerManagerRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(3, 100).WithMessage("First name must be between 3 and 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(3, 100).WithMessage("Last name must be between 3 and 100 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");
        }
    }
}

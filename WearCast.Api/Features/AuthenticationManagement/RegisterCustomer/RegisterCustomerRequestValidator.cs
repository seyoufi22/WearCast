using WearCast.Api.Common.Consts;

namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    public class RegisterCustomerRequestValidator : AbstractValidator<RegisterCustomerRequest>
    {
        public RegisterCustomerRequestValidator() 
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");

            RuleFor(x => x.FirstName)
               .NotEmpty()
               .Length(3, 100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 100);
        }
    }
}

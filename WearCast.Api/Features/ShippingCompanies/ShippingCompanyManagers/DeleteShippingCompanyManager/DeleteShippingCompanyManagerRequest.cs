using WearCast.Api.Features.Admins.DeleteAdmin.DTOs;

namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.DeleteShippingCompanyManager
{
    public class DeleteShippingCompanyManagerRequest : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteShippingCompanyManagerValidator : AbstractValidator<DeleteShippingCompanyManagerRequest>
    {
        public DeleteShippingCompanyManagerValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Shipping Company Manager ID must be valid.");
        }
    }
}

namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.DeleteShippingCompanyManager;

public record DeleteShippingCompanyManagerRequest(
    int ShippingCompanyManagerId,
    string CurrentUserId,
    bool IsAdmin,
    string Reason
) : IRequest<Result>;

public record DeleteShippingCompanyManagerBody(
    string Reason
);

public class DeleteShippingCompanyManagerValidator : AbstractValidator<DeleteShippingCompanyManagerRequest>
{
    public DeleteShippingCompanyManagerValidator()
    {
        RuleFor(x => x.ShippingCompanyManagerId)
            .GreaterThan(0)
            .WithMessage("Shipping Company Manager ID must be valid.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Deletion reason is required.");
    }
}
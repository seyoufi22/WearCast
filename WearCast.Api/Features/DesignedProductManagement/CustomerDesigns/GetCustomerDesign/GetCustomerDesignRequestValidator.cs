namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.GetCustomerDesign
{
    public class GetCustomerDesignRequestValidator : AbstractValidator<GetCustomerDesignRequest>
    {
        public GetCustomerDesignRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid Design Id");
        }
    }
}

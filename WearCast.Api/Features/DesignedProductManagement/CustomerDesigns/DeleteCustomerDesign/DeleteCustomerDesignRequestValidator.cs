namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.DeleteCustomerDesign
{
    public class DeleteCustomerDesignRequestValidator : AbstractValidator<DeleteCustomerDesignRequest>
    {
        public DeleteCustomerDesignRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid Design Id");
        }
    }
}

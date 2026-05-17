namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.DeleteCustomerDesignImage
{
    public class DeleteCustomerDesignImageRequestValidator : AbstractValidator<DeleteCustomerDesignImageRequest>
    {
        public DeleteCustomerDesignImageRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid Design Id.");

            RuleFor(x => x.Side)
                .IsInEnum()
                .WithMessage("Invalid view side. Please choose a valid side (Front, Back, Right, Left).");
        }
    }
}

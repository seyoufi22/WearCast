using WearCast.Api.Features.Platform.UpdateCommission.DTOs;

namespace WearCast.Api.Features.Platform.UpdateCommission.Command;

public class UpdateCommissionValidator : AbstractValidator<UpdateCommissionRequest>
{
    public UpdateCommissionValidator()
    {
        RuleFor(x => x.CommissionPercentage)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Commission percentage must be at least 0%.")
            .LessThanOrEqualTo(100)
            .WithMessage("Commission percentage cannot exceed 100%.");
    }
}

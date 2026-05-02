namespace WearCast.Api.Features.Platform.UpdateCommission.DTOs;

public class UpdateCommissionRequest : IRequest<Result>
{
    public decimal CommissionPercentage { get; set; }
}

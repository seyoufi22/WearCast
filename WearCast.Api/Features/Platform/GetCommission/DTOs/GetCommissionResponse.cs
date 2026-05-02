namespace WearCast.Api.Features.Platform.GetCommission.DTOs;

public record GetCommissionResponse(
    decimal CommissionPercentage,
    DateTime UpdatedOn
);

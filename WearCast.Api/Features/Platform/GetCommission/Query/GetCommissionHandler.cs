using WearCast.Api.Features.Platform.GetCommission.DTOs;

namespace WearCast.Api.Features.Platform.GetCommission.Query;

public class GetCommissionHandler(ApplicationDbContext context)
    : IRequestHandler<GetCommissionRequest, Result<GetCommissionResponse>>
{
    public async Task<Result<GetCommissionResponse>> Handle(GetCommissionRequest request, CancellationToken cancellationToken)
    {
        var settings = await context.PlatformSettings.FirstOrDefaultAsync(cancellationToken);

        if (settings == null)
        {
            return Result.Failure<GetCommissionResponse>(
                new Error("PlatformSettings.NotFound", "Platform settings have not been configured.", StatusCodes.Status404NotFound));
        }

        return Result.Success(new GetCommissionResponse(
            settings.CommissionPercentage,
            settings.UpdatedOn
        ));
    }
}

using WearCast.Api.Features.Platform.UpdateCommission.DTOs;

namespace WearCast.Api.Features.Platform.UpdateCommission.Command;

public class UpdateCommissionHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<UpdateCommissionRequest, Result>
{
    public async Task<Result> Handle(UpdateCommissionRequest request, CancellationToken cancellationToken)
    {
        var settings = await context.PlatformSettings.FirstOrDefaultAsync(cancellationToken);

        if (settings == null)
        {
            // Create platform settings if they don't exist yet
            settings = new PlatformSettings
            {
                CommissionPercentage = request.CommissionPercentage,
                UpdatedById = httpContextAccessor.HttpContext?.User.GetUserId(),
                UpdatedOn = DateTime.UtcNow
            };
            context.PlatformSettings.Add(settings);
        }
        else
        {
            settings.CommissionPercentage = request.CommissionPercentage;
            settings.UpdatedById = httpContextAccessor.HttpContext?.User.GetUserId();
            settings.UpdatedOn = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

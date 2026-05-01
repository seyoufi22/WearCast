using MediatR;

using WearCast.Api.Features.Shipments.AdminAndManager.GetShippingStats.DTOs;

namespace WearCast.Api.Features.Shipments.AdminAndManager.GetShippingStats
{
    public class GetShippingStatsRequest : IRequest<Result<ShippingStatsResponse>>
    {
    }
}

using WearCast.Api.Features.Common.DTOs;
using MediatR;

namespace WearCast.Api.Features.Sellers.GetDashboardStats;

public class GetSellerDashboardStatsRequest : IRequest<Result<SellerDashboardStatsResponse>>
{
}
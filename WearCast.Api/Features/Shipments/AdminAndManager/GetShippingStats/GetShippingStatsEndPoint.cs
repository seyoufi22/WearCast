using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Common.Consts;
using WearCast.Api.Features.Shipments.AdminAndManager.GetShippingStats.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace WearCast.Api.Features.Shipments.AdminAndManager.GetShippingStats
{
    [ApiController]
    [Tags("Shipments")]
    [Route("api/Shipments")]
    public class GetShippingStatsEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetShippingStatsEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin}")]
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetShippingStatsRequest(), cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

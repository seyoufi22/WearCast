using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Drivers.Dashboard.DTOs;

namespace WearCast.Api.Features.Drivers.Dashboard
{
    [Route("api/Drivers/Dashboard")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Driver)]
    [Tags("Drivers")]
    public class DriverDashboardEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public DriverDashboardEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var driverId = User.GetDriverId();
            if (!driverId.HasValue)
            {
                return Unauthorized();
            }


            var result = await _sender.Send(new DriverDashboardRequestDTO { DriverId=driverId.Value}, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

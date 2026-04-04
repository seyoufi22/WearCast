using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Drivers.GetDriverById.DTOs;
using WearCast.Api.Features.Drivers.GetShipmentsByDriverId.DTOs;

namespace WearCast.Api.Features.Drivers.GetShipmentsByDriverId
{
    [ApiController]
    [Tags("Drivers")]
    [Route("api/Drivers")]
    public class GetShipmentsByDriverIdEndPoint : ControllerBase
    {
        private readonly ISender _sender;
        public GetShipmentsByDriverIdEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet("{driverId}/shipments")]
        public async Task<IActionResult> GetByDriverId([FromRoute] int driverId, CancellationToken cancellationToken)
        {
            var request = new GetShipmentsByDriverIdRequestDTO { DriverId = driverId }; 
            var result = await _sender.Send(request, cancellationToken);
            if (result.IsFailure)
            {
                return StatusCode(result.Error.StatusCode.Value, result.Error);
            }
            return Ok(result.Value);
        }
    }
}

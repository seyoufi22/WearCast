using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Drivers.GetAllDrivers.DTOs;
using WearCast.Api.Features.Shipments.Driver.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.GetAllShipments
{
    [ApiController]
    [Tags("Shipments")]
    [Route("api/DriverShipments")]
    public class GetAllDriverShipmentsEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAllDriverShipmentsEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.Driver}")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDriverShipmentsRequestDTO request, CancellationToken cancellationToken)
        {
            if (User.IsDriver())
            {
                var driverId = User.GetDriverId();
                if (driverId.HasValue)
                {
                    request.DriverId = driverId.Value;
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "User is a driver but has no DriverId claim.");
                }
            }

            if (request.DriverId <= 0)
            {
                return BadRequest("DriverId is required.");
            }

            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

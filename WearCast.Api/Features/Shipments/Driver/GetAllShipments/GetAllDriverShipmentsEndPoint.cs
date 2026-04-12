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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDriverShipmentsRequestDTO request, CancellationToken cancellationToken)
        {
            if (!User.IsShippingCompanyManager() && !User.IsSuperAdmin())
            {
                var UserId = User.GetDriverId();
                if (!UserId.HasValue || UserId.Value != request.DriverId)
                {
                    return Unauthorized(new { Message = "You are not authorized to do this action" });
                }
            }
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

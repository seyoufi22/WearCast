using WearCast.Api.Features.Shipments.Driver.GetShipmentById.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.GetShipmentById
{
    [ApiController]
    [Tags("Shipments")]
    [Route("api/drivers")]
    public class GetDriverShipmentByIdEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetDriverShipmentByIdEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet("shipments/{ShipmentId}")]
        public async Task<IActionResult> GetById([FromRoute] int ShipmentId, CancellationToken cancellationToken)
        {
            var DriverId = User.GetDriverId();

            if (!DriverId.HasValue)
            {
                return Unauthorized(new { Message = "Driver Id claim is missing from the token." });
            }

            var result = await _sender.Send(new GetDriverShipmentByIdRequestDTO(ShipmentId, DriverId.Value), cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}

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

        [Authorize(Roles = $"{DefaultRoles.Driver}")]
        [HttpGet("shipments/{ShipmentId}")]
        public async Task<IActionResult> GetById([FromRoute] int ShipmentId, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetDriverShipmentByIdRequestDTO(ShipmentId, User.GetDriverId().Value), cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}

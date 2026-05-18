using WearCast.Api.Features.Shipments.GetShipmentOrders.DTOs;

namespace WearCast.Api.Features.Shipments.GetShipmentOrders
{
    [Route("api/Shipments")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin},{DefaultRoles.Driver}")]
    [Tags("Shipments")]
    public class GetShipmentOrdersEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetShipmentOrdersEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{ShipmentId}/Orders")]
        public async Task<IActionResult> Get([FromRoute] int ShipmentId,
            [FromQuery] GetShipmentOrdersRequestDTO request,
            CancellationToken cancellationToken)
        {

            request.ShipmentId = ShipmentId;
            request.DriverId = User.IsDriver() ? User.GetDriverId() : null;

            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}
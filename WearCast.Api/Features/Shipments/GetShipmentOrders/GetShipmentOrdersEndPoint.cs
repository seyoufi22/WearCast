using WearCast.Api.Features.Shipments.GetShipmentOrders.DTOs;

namespace WearCast.Api.Features.Shipments.GetShipmentOrders
{
    [Route("api/ShipmentOrders")]
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

        [HttpGet("{ShipmentId}")]
        public async Task<IActionResult> Get([FromRoute] int ShipmentId, CancellationToken cancellationToken)
        {
            var request = new GetShipmentOrdersRequestDTO
            {
                ShipmentId = ShipmentId,
                DriverId=User.GetDriverId(),
            };
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}
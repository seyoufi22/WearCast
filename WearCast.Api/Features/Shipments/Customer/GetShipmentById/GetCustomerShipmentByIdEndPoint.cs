using WearCast.Api.Features.Shipments.Customer.GetShipmentById.DTOs;

namespace WearCast.Api.Features.Shipments.Customer.GetShipmentById
{
    [Route("api/Customer")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.Customer}")]
    [Tags("Shipments")]
    public class GetCustomerShipmentByIdEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetCustomerShipmentByIdEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("shipments/{ShipmentId}")]
        public async Task<IActionResult> GetById([FromRoute] int ShipmentId, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetCustomerShipmentByIdRequestDTO(ShipmentId, User.GetCustomerId().Value), cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}

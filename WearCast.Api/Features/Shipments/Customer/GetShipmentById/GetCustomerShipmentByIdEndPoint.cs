using WearCast.Api.Features.Shipments.Customer.GetShipmentById.DTOs;

namespace WearCast.Api.Features.Shipments.Customer.GetShipmentById
{
    [ApiController]
    [Tags("Shipments")]
    [Route("api/Customer")]
    public class GetCustomerShipmentByIdEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetCustomerShipmentByIdEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = $"{DefaultRoles.Customer}")]
        [HttpGet("shipments/{ShipmentId}")]
        public async Task<IActionResult> GetById([FromRoute] int ShipmentId, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetCustomerShipmentByIdRequestDTO(ShipmentId, User.GetCustomerId().Value), cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}

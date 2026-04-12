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

        [Authorize]
        [HttpGet("shipments/{ShipmentId}")]
        public async Task<IActionResult> GetById([FromRoute] int ShipmentId, CancellationToken cancellationToken)
        {
            var CustomerId = User.GetCustomerId();

            if (!CustomerId.HasValue)
            {
                return Unauthorized(new { Message = "Customer Id claim is missing from the token." });
            }

            var result = await _sender.Send(new GetCustomerShipmentByIdRequestDTO(ShipmentId, CustomerId.Value), cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}

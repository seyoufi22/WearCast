using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Shipments.AssignShipment.DTOs;

namespace WearCast.Api.Features.Shipments.AssignShipment
{
    [ApiController]
    [Tags("Shipments")]
    [Route("api/Shipments")]
    public class AssignShipmentEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public AssignShipmentEndPoint(ISender sender)
        {
            _sender = sender;
        }
        [Authorize]
        [HttpPut("{shipmentId}/assign")]
        public async Task<IActionResult> AssignDriver(
            [FromRoute] int shipmentId,
            [FromBody] AssignShipmentRequestDTO request, 
            CancellationToken cancellationToken)
        {
            request.ShipmentId= shipmentId;
            var result = await _sender.Send(request, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error); 
            }

            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Shipments.Driver.AssignShipment.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.AssignShipment
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
        [HttpPut("{ShipmentId}/assign")]
        public async Task<IActionResult> AssignDriver(
            [FromRoute] int ShipmentId,
            [FromBody] AssignShipmentRequestDTO request,
            CancellationToken cancellationToken)
        {
            var AssignerId = User.GetUserId();
            if (!User.IsShippingCompanyManager() && !User.IsSuperAdmin())
            {
                return Unauthorized(new { Message = "You are not allowed to do this action" });
            }
            request.ShipmentId = ShipmentId;
            request.AssignerId = AssignerId!;

            var result = await _sender.Send(request, cancellationToken);

            if (result.IsFailure)
            {
                return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
            }

            return NoContent();
        }
    }
}

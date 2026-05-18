using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Shipments.AdminAndManager.AssignShipment.DTOs;

namespace WearCast.Api.Features.Shipments.AdminAndManager.AssignShipment
{
    [Route("api/Shipments")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipments")]
    public class AssignShipmentEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public AssignShipmentEndPoint(ISender sender)
        {
            _sender = sender;
        }
        [HttpPut("{ShipmentId}/assign")]
        public async Task<IActionResult> AssignDriver(
            [FromRoute] int ShipmentId,
            [FromBody] AssignShipmentRequestDTO request,
            CancellationToken cancellationToken)
        {
            var AssignerId = User.GetUserId();
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Shipments.Driver.UnAssignShipment.DTOs;
using WearCast.Api.Features.Shipments.Driver.UpdateShipmentStatus.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.UnAssignShipment
{
    [Route("api/Shipments")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.Driver},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipments")]
    public class UnAssignShipmentEndPoint : ControllerBase
    {
        private readonly ISender _sender;
        public UnAssignShipmentEndPoint(ISender sender)
        {
            _sender = sender;
        }
        [HttpPut("{ShipmentId}/unassign")]
        public async Task<IActionResult> UnAssign(
            [FromRoute] int ShipmentId,
            CancellationToken cancellationToken)
        {
            var Updaterid = User.GetUserId();
            var request = new UnAssignShipmentRequestDTO()
            {
                ShipmentId = ShipmentId,
                UpdaterId = Updaterid!,
                IsAdmin = !User.IsDriver()
            };
            var result = await _sender.Send(request, cancellationToken);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

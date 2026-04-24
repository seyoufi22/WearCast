using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WearCast.Api.Features.Shipments.Driver.UpdateShipmentStatus.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.UpdateShipmentStatus
{
    [ApiController]
    [Tags("Shipments")]
    [Route("api/Shipments")]
    public class UpdateShipmentStatusEndPoint : ControllerBase
    {
        private readonly ISender _sender;
        public UpdateShipmentStatusEndPoint(ISender sender)
        {
            _sender = sender;
        }
        [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.Driver}")]
        [HttpPut("{ShipmentId}/status")]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] int ShipmentId,
            [FromBody] UpdateShipmentStatusRequestDTO request,
            CancellationToken cancellationToken)
        {
            var UpdaterId = User.GetUserId();
            request.ShipmentId = ShipmentId;
            request.UpdaterId = UpdaterId!;
            request.IsAdmin = !User.IsDriver();
            var result = await _sender.Send(request, cancellationToken);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

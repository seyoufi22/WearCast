using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WearCast.Api.Features.Shipments.UpdateShipmentStatus.DTOs;

namespace WearCast.Api.Features.Shipments.UpdateShipmentStatus
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
        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] int id,
            [FromBody] UpdateShipmentStatusRequestDTO request,
            CancellationToken cancellationToken)
        {
            request.ShipmentId = id;
            var result = await _sender.Send(request, cancellationToken);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return StatusCode(result.Error.StatusCode.Value, result.Error);
        }
    }
}

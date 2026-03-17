using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Drivers.ChangeDriverStatus.DTOs;

namespace WearCast.Api.Features.Drivers.ChangeDriverStatus
{
    [ApiController]
    [Tags("Drivers")]
    [Route("api/Drivers/ChangeStatus")]
    public class ChangeDriverStatusEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public ChangeDriverStatusEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] int id,
            [FromBody] UpdateDriverStatusRequestDTO request,
            CancellationToken cancellationToken)
        {
            request.DriverId = id;
            var result = await _sender.Send(request, cancellationToken);

            if (result.IsFailure)
            {
                return StatusCode(result.Error.StatusCode.Value, result.Error);
            }

            return NoContent();
        }
    }
}

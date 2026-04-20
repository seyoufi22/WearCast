using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Drivers.ChangeDriverStatus.DTOs;

namespace WearCast.Api.Features.Drivers.ChangeDriverStatus
{
    [ApiController]
    [Tags("Drivers")]
    [Route("api/Drivers")]
    public class ChangeDriverStatusEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public ChangeDriverStatusEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpPatch("{id}/ChangeStatus")]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] int id,
            [FromBody] UpdateDriverStatusRequestDTO request,
            CancellationToken cancellationToken)
        {
            if (!User.IsShippingCompanyManager() && !User.IsSuperAdmin() && !User.IsDriver())
            {
                return Unauthorized(new { Message = "You are not allowed to do this action" });
            }
            var UpdaterId = User.GetUserId();
            request.DriverId = id;
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

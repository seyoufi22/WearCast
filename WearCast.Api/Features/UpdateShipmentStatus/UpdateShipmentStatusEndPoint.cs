using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WearCast.Api.Features.UpdateShipmentStatus.DTOs;

namespace WearCast.Api.Features.UpdateShipmentStatus
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UpdateShipmentStatusEndPoint : ControllerBase
    {
        private readonly ISender _sender;
        public UpdateShipmentStatusEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpPatch("status")]
        public async Task<IActionResult> UpdateStatus(
            [FromBody] UpdateShipmentStatusRequestDTO request,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role) ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Not authorized" });
            }

            request.UserId = userId;
            request.UserRole = userRole;

            var result = await _sender.Send(request, cancellationToken);

            if (result.IsSuccess) return Ok(result);

            return BadRequest(result);
        }
    }
}

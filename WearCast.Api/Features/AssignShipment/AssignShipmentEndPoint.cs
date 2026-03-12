using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.AssignShipment.DTOs;

namespace WearCast.Api.Features.AssignShipment
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssignShipmentEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public AssignShipmentEndPoint(ISender sender)
        {
            _sender = sender;
        }
        [HttpPost("assign")]
        public async Task<IActionResult> AssignShipment(
            [FromBody] AssignShipmentRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.AuthenticationManagement.GetUserId.DTOs;

namespace WearCast.Api.Features.AuthenticationManagement.GetUserId
{
    [Route("api/auth")]
    [ApiController]
    [Tags("Auth")]
    public class GetUserIdEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetUserIdEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("getid")] 
        public async Task<IActionResult> GetUserId([FromBody] GetUserIdRequestDTO request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

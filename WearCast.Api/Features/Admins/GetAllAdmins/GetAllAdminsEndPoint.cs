using WearCast.Api.Features.Admins.GetAllAdmins.DTOs;

namespace WearCast.Api.Features.Admins.GetAllAdmins
{
    [ApiController]
    [Tags("Admins")]
    [Route("api/Admins/GetAll")]
    public class GetAllAdminsEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAllAdminsEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin}")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllAdminsRequestDTO request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

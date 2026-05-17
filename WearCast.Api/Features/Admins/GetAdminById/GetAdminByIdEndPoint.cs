using WearCast.Api.Features.Admins.GetAdminById.DTOs;

namespace WearCast.Api.Features.Admins.GetAdminById
{
    [ApiController]
    [Tags("Admins")]
    [Route("api/Admins")]
    public class GetAdminByIdEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAdminByIdEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetAdminByIdRequestDTO { Id = id }, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}
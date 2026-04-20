using WearCast.Api.Features.Drivers.GetDriverById.DTOs;

namespace WearCast.Api.Features.Drivers.GetDriverById
{
    [ApiController]
    [Tags("Drivers")]
    [Route("api/Drivers")]
    public class GetDriverByIdEndPoint : ControllerBase
    {
        private readonly ISender _sender;
        public GetDriverByIdEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet("{id}/GetById")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            if (!User.IsShippingCompanyManager() && !User.IsSuperAdmin() && !User.IsDriver())
            {
                return Unauthorized(new { Message = "You are not allowed to do this action" });
            }
            if (User.IsDriver())
            {
                if (User.GetDriverId() != id)
                {
                    return Unauthorized(new { Message = "You are not allowed to do this action" });
                }
            }
            var result = await _sender.Send(new GetDriverByIdRequestDTO(id), cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}

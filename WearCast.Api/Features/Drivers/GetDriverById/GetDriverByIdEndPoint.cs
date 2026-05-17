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

        [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.Driver}")]
        [HttpGet("{id}/GetById")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            if (User.IsDriver() && User.GetDriverId() != id)
            {
                return (IActionResult)Result.Failure(AuthErrors.Forbidden);
            }

            var result = await _sender.Send(new GetDriverByIdRequestDTO(id), cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}

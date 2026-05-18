using WearCast.Api.Features.Drivers.GetDriverOrders.DTOs;

namespace WearCast.Api.Features.Drivers.GetDriverOrders
{
    [Route("api/Driver/Orders")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin},{DefaultRoles.Driver}")]
    [Tags("Driver Orders")]
    public class GetDriverOrdersEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetDriverOrdersEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] GetDriverOrdersRequestDTO request,
            CancellationToken cancellationToken)
        {
            if (User.IsDriver())
            {
                if (User.GetDriverId() != request.DriverId)
                {
                    return Unauthorized();
                }
            }
            
            var result = await _sender.Send(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

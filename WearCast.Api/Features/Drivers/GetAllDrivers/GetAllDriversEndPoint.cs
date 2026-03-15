using WearCast.Api.Features.Drivers.GetAllDrivers.DTOs;

namespace WearCast.Api.Features.Drivers.GetAllDrivers
{
   
    [ApiController]
    [Tags("Drivers")]
    [Route("api/Drivers/GetAll")]
    public class GetAllDriversEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAllDriversEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetAllDriversRequestDTO(), cancellationToken);

            return Ok(result);
        }
    }
}

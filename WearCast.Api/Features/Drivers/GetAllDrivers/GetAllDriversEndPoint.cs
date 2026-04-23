using WearCast.Api.Features.Drivers.GetAllDrivers.DTOs;
using WearCast.Api.Features.Shipments.AdminAndManager.GetAllShipments.DTOs;

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
        public async Task<IActionResult> GetAll([FromQuery] GetAllDriversRequestDTO request, CancellationToken cancellationToken)
        {
            if (!User.IsShippingCompanyManager() && !User.IsSuperAdmin())
            {
                return Unauthorized(new { Message = "You are not authorized to do this action" });
            }
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

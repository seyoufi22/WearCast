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

        [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin}")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDriversRequestDTO request, CancellationToken cancellationToken)
        {

            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

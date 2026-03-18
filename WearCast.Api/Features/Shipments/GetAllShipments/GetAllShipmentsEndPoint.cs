using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Drivers.GetAllDrivers.DTOs;
using WearCast.Api.Features.Shipments.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Shipments.GetAllShipments
{
    [ApiController]
    [Tags("Shipments")]
    [Route("api/Shipments")]
    public class GetAllShipmentsEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAllShipmentsEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetAllShipmentsRequestDTO(), cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

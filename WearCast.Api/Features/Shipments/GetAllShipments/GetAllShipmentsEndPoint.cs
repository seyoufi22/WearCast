using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Drivers.GetAllDrivers.DTOs;
using WearCast.Api.Features.Shipments.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Shipments.GetAllShipments
{
    [Route("api/[controller]")]
    [ApiController]
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

            return Ok(result);
        }
    }
}

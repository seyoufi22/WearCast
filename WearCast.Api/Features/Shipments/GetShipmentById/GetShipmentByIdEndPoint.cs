using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Drivers.GetDriverById.DTOs;
using WearCast.Api.Features.Shipments.GetShipmentById.DTOs;

namespace WearCast.Api.Features.Shipments.GetShipmentById
{
    [ApiController]
    [Tags("Shipments")]
    [Route("api/Shipments")]
    public class GetShipmentByIdEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetShipmentByIdEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetShipmentByIdRequestDTO(id), cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}

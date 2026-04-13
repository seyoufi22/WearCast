using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Shipments.AdminAndManager.GetShipmentById.DTOs;
using WearCast.Api.Features.Shipments.Customer.GetShipmentById.DTOs;

namespace WearCast.Api.Features.Shipments.AdminAndManager.GetShipmentById
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
        [HttpGet("{ShipmentId}")]
        public async Task<IActionResult> GetById([FromRoute] int ShipmentId, CancellationToken cancellationToken)
        {
            if (!User.IsShippingCompanyManager() && !User.IsSuperAdmin())
            {
                return Unauthorized(new { Message = "You are not authorized to do this action" });
            }

            var result = await _sender.Send(new GetShipmentByIdRequestDTO(ShipmentId), cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}

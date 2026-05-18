using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Shipments.AdminAndManager.GetShipmentById.DTOs;
using WearCast.Api.Features.Shipments.Customer.GetShipmentById.DTOs;

namespace WearCast.Api.Features.Shipments.AdminAndManager.GetShipmentById
{
    [Route("api/Shipments")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipments")]
    public class GetShipmentByIdEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetShipmentByIdEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{ShipmentId}")]
        public async Task<IActionResult> GetById([FromRoute] int ShipmentId, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetShipmentByIdRequestDTO(ShipmentId), cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}

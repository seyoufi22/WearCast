using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Shipments.AdminAndManager.GetAllShipments.DTOs;
using WearCast.Api.Features.Shipments.Customer.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Shipments.AdminAndManager.GetAllShipments
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
        public async Task<IActionResult> GetAll([FromQuery] GetAllShipmentsRequestDTO request, CancellationToken cancellationToken)
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

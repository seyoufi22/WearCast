using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Shipments.Customer.GetAllShipments.DTOs;
using WearCast.Api.Features.Shipments.Driver.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Shipments.Customer.GetAllShipments
{
    [ApiController]
    [Tags("Shipments")]
    [Route("api/CustomerShipments")]
    public class GetAllCustomerShipmentsEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAllCustomerShipmentsEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.Customer}")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCustomerShipmentsRequestDTO request, CancellationToken cancellationToken)
        {
            if (User.IsCustomer() && User.GetCustomerId() != request.CustomerId)
            {
                return (IActionResult)Result.Failure(AuthErrors.Forbidden);
            }

            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

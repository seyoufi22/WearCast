using WearCast.Api.Features.Shipments.Customer.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Shipments.Customer.GetAllShipments
{
    [Route("api/CustomerShipments")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.Customer},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipments")]
    public class GetAllCustomerShipmentsEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAllCustomerShipmentsEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCustomerShipmentsRequestDTO request, CancellationToken cancellationToken)
        {
            if (User.IsCustomer() && User.GetCustomerId() != request.CustomerId)
            {
                return Unauthorized();
            }

            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

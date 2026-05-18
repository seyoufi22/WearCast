using WearCast.Api.Features.Shipments.Driver.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.GetAllShipments
{
    [Route("api/DriverShipments")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.Driver},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipments")]
    public class GetAllDriverShipmentsEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAllDriverShipmentsEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDriverShipmentsRequestDTO request, CancellationToken cancellationToken)
        {
            if (User.IsDriver() && User.GetDriverId() != request.DriverId)
            {
                return (IActionResult)Result.Failure(AuthErrors.Forbidden);
            }

            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

using WearCast.Api.Features.ShippingCompanies.GetAllOrders.DTOs;

namespace WearCast.Api.Features.ShippingCompanies.GetAllOrders
{
    [Route("api/ShippingCompany/Orders")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipping Company Orders")]
    public class GetAllOrdersEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAllOrdersEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllOrdersRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

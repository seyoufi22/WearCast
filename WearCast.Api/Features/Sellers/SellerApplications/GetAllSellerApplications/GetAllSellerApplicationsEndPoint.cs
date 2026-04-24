using WearCast.Api.Features.Sellers.SellerApplications.GetAllSellerApplications.DTOs;

namespace WearCast.Api.Features.Sellers.SellerApplications.GetAllSellerApplications
{
    [ApiController]
    [Tags("Seller Applications")]
    [Route("api/seller-applications/GetAll")]
    public class GetAllSellerApplicationsEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAllSellerApplicationsEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin}")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllSellerApplicationsRequestDTO request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
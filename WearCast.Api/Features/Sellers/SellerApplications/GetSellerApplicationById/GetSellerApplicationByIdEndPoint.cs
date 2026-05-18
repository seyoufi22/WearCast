using WearCast.Api.Features.Sellers.SellerApplications.GetSellerApplicationById.DTOs;

namespace WearCast.Api.Features.SellerApplications.AdminAndManager.GetSellerApplicationById
{
    [ApiController]
    [Tags("Seller Applications")]
    [Route("api/seller-applications")]
    public class GetSellerApplicationByIdEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetSellerApplicationByIdEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.VendorAdmin}")]
        [HttpGet("{ApplicationId}")]
        public async Task<IActionResult> GetById([FromRoute] int ApplicationId, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetSellerApplicationByIdRequestDTO(ApplicationId), cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}
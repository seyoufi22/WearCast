using System.Security.Claims;

namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.DeleteShippingCompanyManager
{
    [ApiController]
    [Route("api/shipping-company-managers")] 
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipping Company Manager Profile")]
    public class DeleteShippingCompanyManagerEndpoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{shippingCompanyManagerId:int}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int shippingCompanyManagerId,
            [FromBody] DeleteShippingCompanyManagerBody body,
            CancellationToken cancellationToken)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            bool isAdmin = User.IsInRole(DefaultRoles.SuperAdmin) || User.IsInRole(DefaultRoles.OperationsAdmin);

            var request = new DeleteShippingCompanyManagerRequest(shippingCompanyManagerId, currentUserId, isAdmin, body.Reason);
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
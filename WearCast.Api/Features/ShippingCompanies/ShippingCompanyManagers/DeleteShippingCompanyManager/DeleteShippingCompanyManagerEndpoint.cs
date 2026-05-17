using WearCast.Api.Features.Admins.DeleteAdmin.DTOs;

namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.DeleteShippingCompanyManager
{
    [ApiController]
    [Route("api/shipping-company-managers/Delete")]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipping Company Manager Profile")]
    public class DeleteShippingCompanyManagerEndpoint : ControllerBase
    {
        private readonly ISender _sender;

        public DeleteShippingCompanyManagerEndpoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var request = new DeleteShippingCompanyManagerRequest { Id = id };
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? NoContent() :
                StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

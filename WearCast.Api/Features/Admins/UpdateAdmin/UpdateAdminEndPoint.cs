namespace WearCast.Api.Features.Admins.UpdateAdmin
{
    [Route("api/admins")]
    [ApiController]
    [Tags("Admin Management")]
    [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin},{DefaultRoles.VendorAdmin},{DefaultRoles.CatalogAdmin},{DefaultRoles.CustomerServiceAdmin}")]

    public class UpdateAdminEndpoint(IMediator mediator) : ControllerBase
    {
        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateAdmin(
            [FromRoute] string id,
            [FromBody] UpdateAdminRequestBody body,
            CancellationToken cancellationToken)
        {
            var request = new UpdateAdminRequest(
                AdminId: id,
                FirstName: body.FirstName,
                LastName: body.LastName,
                PhoneNumber: body.PhoneNumber,
                Role: body.Role
            );

            var result = await mediator.Send(request, cancellationToken);
            return result.ToResponse();
        }
    }
    public record UpdateAdminRequestBody(
        string FirstName,
        string LastName,
        string PhoneNumber,
        AdminRole Role
    );
}

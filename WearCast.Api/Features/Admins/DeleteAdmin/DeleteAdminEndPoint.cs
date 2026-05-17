using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Admins.DeleteAdmin.DTOs;

namespace WearCast.Api.Features.Admins.DeleteAdmin
{
    [ApiController]
    [Tags("Admins")]
    [Route("api/Admins/Delete")]
    public class DeleteAdminEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public DeleteAdminEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
        {
            var request = new DeleteAdminRequestDTO { Id = id };
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? NoContent() :
                StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

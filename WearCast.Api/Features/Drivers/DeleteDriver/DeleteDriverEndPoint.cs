using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.Drivers.DeleteDriver.DTOs;

namespace WearCast.Api.Features.Drivers.DeleteDriver
{
    [ApiController]
    [Tags("Drivers")]
    [Route("api/Drivers")]
    public class DeleteDriverEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public DeleteDriverEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var managerId = User.GetUserId();
            var request = new DeleteDriverRequestDTO 
            { 
                DriverId = id, 
                ManagerId = managerId ?? string.Empty 
            };

            var result = await _sender.Send(request, cancellationToken);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

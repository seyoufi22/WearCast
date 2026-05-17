using WearCast.Api.Features.Drivers.DeleteDriver.DTOs;

namespace WearCast.Api.Features.Drivers.DeleteDriver
{
    [Route("api/drivers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
    [Tags("Drivers")]
    public class DeleteDriverEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public DeleteDriverEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var updaterId = User.GetUserId();

            var request = new DeleteDriverRequestDTO
            {
                DriverId = id,
                UpdaterId = updaterId!
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

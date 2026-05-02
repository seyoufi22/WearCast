using WearCast.Api.Features.NotificationManagement.DeleteNotification.DTOs;

namespace WearCast.Api.Features.NotificationManagement.DeleteNotification
{
    [ApiController]
    [Tags("Notifications")]
    [Route("api/Notifications/Delete")]
    public class DeleteNotificationEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public DeleteNotificationEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var request = new DeleteNotificationRequestDTO { NotificationId = id };
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccess ? NoContent() :
                StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

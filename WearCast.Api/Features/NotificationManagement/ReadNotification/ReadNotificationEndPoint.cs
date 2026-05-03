using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.NotificationManagement.ReadNotification.DTOs;

namespace WearCast.Api.Features.NotificationManagement.ReadNotification
{
    [ApiController]
    [Tags("Notifications")]
    [Route("api/Notifications/Read")]
    public class ReadNotificationEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public ReadNotificationEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpPut("{notificationId}")]
        public async Task<IActionResult> Read(int notificationId, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new ReadNotificationRequestDTO { NotificationId = notificationId }, cancellationToken);

            return result.IsSuccess ? NoContent() :
                         StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

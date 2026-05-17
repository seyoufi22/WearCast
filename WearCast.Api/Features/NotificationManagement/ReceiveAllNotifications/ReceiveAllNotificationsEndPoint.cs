using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.NotificationManagement.ReceiveAllNotifications.DTOs;

namespace WearCast.Api.Features.NotificationManagement.ReceiveAllNotifications
{
    [ApiController]
    [Tags("Notifications")]
    [Route("api/Notifications/ReceiveAll")]
    public class ReceiveAllNotificationsEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public ReceiveAllNotificationsEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ReceiveAll(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new ReceiveAllNotificationsRequestDTO(), cancellationToken);

            return result.IsSuccess ? NoContent() :
                         StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

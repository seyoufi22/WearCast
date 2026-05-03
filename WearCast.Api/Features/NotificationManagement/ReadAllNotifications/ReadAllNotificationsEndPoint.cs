using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.NotificationManagement.ReadAllNotifications.DTOs;

namespace WearCast.Api.Features.NotificationManagement.ReadAllNotifications
{
    [ApiController]
    [Tags("Notifications")]
    [Route("api/Notifications/ReadAll")]
    public class ReadAllNotificationsEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public ReadAllNotificationsEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ReadAll(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new ReadAllNotificationsRequestDTO(), cancellationToken);

            return result.IsSuccess ? NoContent() :
                         StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
    }
}

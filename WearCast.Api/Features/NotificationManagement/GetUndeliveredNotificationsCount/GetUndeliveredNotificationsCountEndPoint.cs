using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.NotificationManagement.GetUndeliveredNotificationsCount.DTOs;

namespace WearCast.Api.Features.NotificationManagement.GetUndeliveredNotificationsCount
{
    [ApiController]
    [Tags("Notifications")]
    [Route("api/Notifications/UndeliveredCount")]
    public class GetUndeliveredNotificationsCountEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetUndeliveredNotificationsCountEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCount(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUndeliveredNotificationsCountRequestDTO(), cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

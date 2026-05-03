using WearCast.Api.Features.NotificationManagement.GetAllNotifications.DTOs;

namespace WearCast.Api.Features.NotificationManagement.GetAllNotifications
{
    [ApiController]
    [Tags("Notifications")]
    [Route("api/Notifications/GetAll")]
    public class GetAllNotificationsEndPoint : ControllerBase
    {
        private readonly ISender _sender;

        public GetAllNotificationsEndPoint(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllNotificationsRequestDTO request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}

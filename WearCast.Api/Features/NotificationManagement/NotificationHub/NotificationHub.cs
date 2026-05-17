using Microsoft.AspNetCore.SignalR;

namespace WearCast.Api.Features.NotificationManagement.NotificationHub
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;

            Console.WriteLine($"User connected: {userId}");

            await base.OnConnectedAsync();
        }
    }
}

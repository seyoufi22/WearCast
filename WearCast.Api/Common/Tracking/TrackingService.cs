using Hangfire;
using System.Text.Json;
using WearCast.Api.Common.Tracking.Models;

namespace WearCast.Api.Common.Tracking
{
    public class TrackingService(IServiceProvider serviceProvider) : ITrackingService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public void TrackClick(ClickEvent e) =>
            BackgroundJob.Enqueue(() => SaveToDb(e.UserId, JsonSerializer.Serialize(e, _options)));

        public void TrackFilter(FilterEvent e) =>
            BackgroundJob.Enqueue(() => SaveToDb(e.UserId, JsonSerializer.Serialize(e, _options)));

        public void TrackPurchase(PurchaseEvent e) =>
            BackgroundJob.Enqueue(() => SaveToDb(e.UserId, JsonSerializer.Serialize(e, _options)));

        [AutomaticRetry(Attempts = 2)]
        public async Task SaveToDb(string userId, string payload)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            db.UserActivityLogs.Add(new UserActivityLog
            {
                UserId = userId,
                Payload = payload
            });

            await db.SaveChangesAsync();
        }
    }
}

namespace WearCast.Api.Entities
{
    public class UserActivityLog
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Payload { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

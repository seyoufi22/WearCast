namespace WearCast.Api.Persistence.EntitiesConfigurations.NotificationConfiguration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.HasOne(n => n.User)
               .WithMany(u => u.Notifications)
               .HasForeignKey(n => n.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(n => !n.IsDeleted);

            builder.HasIndex(n => new { n.UserId, n.IsRead });
        }
    }
}

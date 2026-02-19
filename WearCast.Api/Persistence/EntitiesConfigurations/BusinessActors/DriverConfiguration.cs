
namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.HasOne(d => d.ApplicationUser)
                 .WithOne()
                 .HasForeignKey<Driver>(d => d.UserId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

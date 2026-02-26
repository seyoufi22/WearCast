
namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.ApplicationUser)
                 .WithOne()
                 .HasForeignKey<Driver>(d => d.UserId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.ShippingCompany)
                   .WithMany(c => c.Drivers)
                   .HasForeignKey(d => d.ShippingCompanyId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

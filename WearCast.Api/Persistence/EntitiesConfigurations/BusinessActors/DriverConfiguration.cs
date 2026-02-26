
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

            builder.Property(x => x.NationalId)
                .IsRequired()
                .HasMaxLength(14)
                .IsFixedLength()
                .IsUnicode(false);

            builder.HasIndex(x => x.NationalId)
                .IsUnique();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.VehicleType)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.VehiclePlateNumber)
                .HasMaxLength(20)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(x => x.ProfileImageUrl)
                .HasMaxLength(500)
                .HasDefaultValue(string.Empty);
        }
    }
}


namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.HasOne(d => d.ApplicationUser)
                 .WithOne(u => u.Driver)
                 .HasForeignKey<Driver>(d => d.UserId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.ShippingCompany)
                   .WithMany(sh => sh.Drivers)
                   .HasForeignKey(d => d.ShippingCompanyId)
                   .OnDelete(DeleteBehavior.Cascade);

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

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.State).IsRequired().HasMaxLength(50).HasColumnName("State");
                address.Property(a => a.City).IsRequired().HasMaxLength(50).HasColumnName("City");
                address.Property(a => a.Street).IsRequired().HasMaxLength(200).HasColumnName("Street");
                address.Property(a => a.BuildingNumber).IsRequired().HasMaxLength(20).HasColumnName("BuildingNumber");
            });
        }
    }
}

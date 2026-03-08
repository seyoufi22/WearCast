
namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors.Sellers
{
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.CommercialRegisterNumber).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.CommercialRegisterNumber).IsUnique();

            builder.Property(x => x.TaxIdNumber).IsRequired().HasMaxLength(9);
            builder.HasIndex(x => x.TaxIdNumber).IsUnique();

            builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);

            builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.LogoUrl).IsRequired().HasMaxLength(500);

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

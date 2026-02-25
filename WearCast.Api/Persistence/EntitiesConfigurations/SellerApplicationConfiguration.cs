
namespace WearCast.Api.Persistence.EntitiesConfigurations
{
    public class SellerApplicationConfiguration : IEntityTypeConfiguration<SellerApplication>
    {
        public void Configure(EntityTypeBuilder<SellerApplication> builder)
        {
            builder.ToTable("SellerApplications");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);


            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(x => x.Email)
                .IsUnique()
                .HasFilter("[Status] != 3");

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(11);


            builder.HasIndex(x => x.PhoneNumber)
                   .IsUnique()
                   .HasFilter("[Status] != 3");

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.SellerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.CommercialRegisterNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.TaxIdNumber)
                .IsRequired()
                .HasMaxLength(9);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.LogoUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.OwnsOne(x => x.StoreAddress, address =>
            {
                address.Property(a => a.State)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("State");

                address.Property(a => a.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("City");

                address.Property(a => a.Street)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Street");

                address.Property(a => a.BuildingNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("BuildingNumber");
            });

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.RejectionReason)
                .IsRequired(false)
                .HasMaxLength(500);
        }
    }
}

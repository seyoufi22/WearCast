namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors.Sellers
{
    public class SellerApplicationConfiguration : IEntityTypeConfiguration<SellerApplication>
    {
        public void Configure(EntityTypeBuilder<SellerApplication> builder)
        {
            builder.ToTable("SellerApplications");
            builder.HasKey(x => x.Id);


            builder.Property(x => x.ManagerFirstName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ManagerLastName).IsRequired().HasMaxLength(50);

            builder.Property(x => x.ManagerEmail).IsRequired().HasMaxLength(256);
            builder.HasIndex(x => x.ManagerEmail)
                .IsUnique()
                .HasFilter("[Status] != 3");

            builder.Property(x => x.ManagerPhoneNumber).IsRequired().HasMaxLength(11);
            builder.HasIndex(x => x.ManagerPhoneNumber)
                .IsUnique()
                .HasFilter("[Status] != 3");

            builder.Property(x => x.ManagerPasswordHash).IsRequired();


            builder.Property(x => x.ManagerEmailConfirmationCode)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(x => x.ManagerEmailConfirmed)
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedOn).IsRequired();


            builder.Property(x => x.SellerName).IsRequired().HasMaxLength(100);

            builder.HasIndex(x => x.SellerName)
               .IsUnique()
               .HasFilter("[Status] != 3");

            builder.Property(x => x.SellerEmail).IsRequired().HasMaxLength(256);
            builder.HasIndex(x => x.SellerEmail)
                .IsUnique()
                .HasFilter("[Status] != 3");

            builder.Property(x => x.SellerPhoneNumber).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.SellerPhoneNumber)
                .IsUnique()
                .HasFilter("[Status] != 3");

            builder.Property(x => x.CommercialRegisterNumber).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.CommercialRegisterNumber)
                .IsUnique()
                .HasFilter("[Status] != 3");

            builder.Property(x => x.TaxIdNumber).IsRequired().HasMaxLength(9);
            builder.HasIndex(x => x.TaxIdNumber)
                .IsUnique()
                .HasFilter("[Status] != 3");

            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.LogoUrl).IsRequired().HasMaxLength(500);

            builder.OwnsOne(x => x.SellerAddress, address =>
            {
                address.Property(a => a.State).IsRequired().HasMaxLength(50).HasColumnName("State");
                address.Property(a => a.City).IsRequired().HasMaxLength(50).HasColumnName("City");
                address.Property(a => a.Street).IsRequired().HasMaxLength(200).HasColumnName("Street");
                address.Property(a => a.BuildingNumber).IsRequired().HasMaxLength(20).HasColumnName("BuildingNumber");
            });


            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.RejectionReason).IsRequired(false).HasMaxLength(500);
        }
    }
}
namespace WearCast.Api.Persistence.EntitiesConfigurations.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Country");

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

                address.HasData(
                    new { ApplicationUserId = DefaultUsers.SuperAdminId, Country = "Egypt", State = "Cairo", City = "Nasr City", Street = "Makram Ebeid", BuildingNumber = "10" },
                    new { ApplicationUserId = DefaultUsers.CustomerId, Country = "Egypt", State = "Alexandria", City = "Smouha", Street = "Victor Emmanuel", BuildingNumber = "15" },
                    new { ApplicationUserId = DefaultUsers.SellerId, Country = "Egypt", State = "Giza", City = "Dokki", Street = "Tahrir St", BuildingNumber = "20" },
                    new { ApplicationUserId = DefaultUsers.FactoryId, Country = "Egypt", State = "Sharqia", City = "10th of Ramadan", Street = "Industrial Zone", BuildingNumber = "50" },
                    new { ApplicationUserId = DefaultUsers.ShippingCompanyId, Country = "Egypt", State = "Cairo", City = "Maadi", Street = "Road 9", BuildingNumber = "5" },
                    new { ApplicationUserId = DefaultUsers.DriverId, Country = "Egypt", State = "Cairo", City = "Heliopolis", Street = "El Hegaz", BuildingNumber = "30" }
                );
            });


            builder
               .OwnsMany(x => x.RefreshTokens)
               .ToTable("RefreshTokens")
               .WithOwner()
               .HasForeignKey("UserId");

            builder.Property(x => x.FirstName).HasMaxLength(100);
            builder.Property(x => x.LastName).HasMaxLength(100);

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.NormalizedEmail).IsUnique();

            //SeedingData

            var passwordHasher = new PasswordHasher<ApplicationUser>();


            var seedUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = DefaultUsers.SuperAdminId,
                    FirstName = "WearCast",
                    LastName = "SuperAdmin",
                    UserName = DefaultUsers.SuperAdminEmail,
                    NormalizedUserName = DefaultUsers.SuperAdminEmail.ToUpper(),
                    Email = DefaultUsers.SuperAdminEmail,
                    NormalizedEmail = DefaultUsers.SuperAdminEmail.ToUpper(),
                    SecurityStamp = DefaultUsers.SuperAdminSecurityStamp,
                    ConcurrencyStamp = DefaultUsers.SuperAdminConcurrencyStamp,
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.SuperAdminPassword),
                    Address = null!
                },
                new ApplicationUser
                {
                    Id = DefaultUsers.CustomerId,
                    FirstName = "WearCast",
                    LastName = "Customer",
                    UserName = DefaultUsers.CustomerEmail,
                    NormalizedUserName = DefaultUsers.CustomerEmail.ToUpper(),
                    Email = DefaultUsers.CustomerEmail,
                    NormalizedEmail = DefaultUsers.CustomerEmail.ToUpper(),
                    SecurityStamp = DefaultUsers.CustomerSecurityStamp,
                    ConcurrencyStamp = DefaultUsers.CustomerConcurrencyStamp,
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.CustomerPassword),
                    Address = null!
                },
                new ApplicationUser
                {
                    Id = DefaultUsers.SellerId,
                    FirstName = "WearCast",
                    LastName = "Seller",
                    UserName = DefaultUsers.SellerEmail,
                    NormalizedUserName = DefaultUsers.SellerEmail.ToUpper(),
                    Email = DefaultUsers.SellerEmail,
                    NormalizedEmail = DefaultUsers.SellerEmail.ToUpper(),
                    SecurityStamp = DefaultUsers.SellerSecurityStamp,
                    ConcurrencyStamp = DefaultUsers.SellerConcurrencyStamp,
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.SellerPassword),
                    Address = null!
                },
                new ApplicationUser
                {
                    Id = DefaultUsers.FactoryId,
                    FirstName = "WearCast",
                    LastName = "Factory",
                    UserName = DefaultUsers.FactoryEmail,
                    NormalizedUserName = DefaultUsers.FactoryEmail.ToUpper(),
                    Email = DefaultUsers.FactoryEmail,
                    NormalizedEmail = DefaultUsers.FactoryEmail.ToUpper(),
                    SecurityStamp = DefaultUsers.FactorySecurityStamp,
                    ConcurrencyStamp = DefaultUsers.FactoryConcurrencyStamp,
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.FactoryPassword),
                    Address = null!
                },
                new ApplicationUser
                {
                    Id = DefaultUsers.ShippingCompanyId,
                    FirstName = "WearCast",
                    LastName = "ShippingCompany",
                    UserName = DefaultUsers.ShippingCompanyEmail,
                    NormalizedUserName = DefaultUsers.ShippingCompanyEmail.ToUpper(),
                    Email = DefaultUsers.ShippingCompanyEmail,
                    NormalizedEmail = DefaultUsers.ShippingCompanyEmail.ToUpper(),
                    SecurityStamp = DefaultUsers.ShippingCompanySecurityStamp,
                    ConcurrencyStamp = DefaultUsers.ShippingCompanyConcurrencyStamp,
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.ShippingCompanyPassword),
                    Address = null!
                },
                new ApplicationUser
                {
                    Id = DefaultUsers.DriverId,
                    FirstName = "WearCast",
                    LastName = "Driver",
                    UserName = DefaultUsers.DriverEmail,
                    NormalizedUserName = DefaultUsers.DriverEmail.ToUpper(),
                    Email = DefaultUsers.DriverEmail,
                    NormalizedEmail = DefaultUsers.DriverEmail.ToUpper(),
                    SecurityStamp = DefaultUsers.DriverSecurityStamp,
                    ConcurrencyStamp = DefaultUsers.DriverConcurrencyStamp,
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.DriverPassword),
                    Address = null!
                }

            };

            builder.HasData(seedUsers);
        }
    }
}

namespace WearCast.Api.Persistence.EntitiesConfigurations.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.IsDisabled)
                .HasDefaultValue(false);

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired(true);

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.NormalizedEmail).IsUnique();

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.State).IsRequired().HasMaxLength(50).HasColumnName("State");
                address.Property(a => a.City).IsRequired().HasMaxLength(50).HasColumnName("City");
                address.Property(a => a.Street).IsRequired().HasMaxLength(200).HasColumnName("Street");
                address.Property(a => a.BuildingNumber).IsRequired().HasMaxLength(20).HasColumnName("BuildingNumber");

                address.HasData(
                    new { ApplicationUserId = DefaultUsers.SuperAdminId, State = "Cairo", City = "Nasr City", Street = "Makram Ebeid", BuildingNumber = "10" },
                    new { ApplicationUserId = DefaultUsers.CustomerId, State = "Alexandria", City = "Smouha", Street = "Victor Emmanuel", BuildingNumber = "15" },
                    new { ApplicationUserId = DefaultUsers.SellerId, State = "Giza", City = "Dokki", Street = "Tahrir St", BuildingNumber = "20" },
                    new { ApplicationUserId = DefaultUsers.FactoryId, State = "Sharqia", City = "10th of Ramadan", Street = "Industrial Zone", BuildingNumber = "50" },
                    new { ApplicationUserId = DefaultUsers.ShippingCompanyId, State = "Cairo", City = "Maadi", Street = "Road 9", BuildingNumber = "5" },
                    new { ApplicationUserId = DefaultUsers.DriverId, State = "Cairo", City = "Heliopolis", Street = "El Hegaz", BuildingNumber = "30" }
                );
            });

            builder.OwnsMany(x => x.RefreshTokens)
                .ToTable("RefreshTokens")
                .WithOwner()
                .HasForeignKey("UserId");

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
                    PhoneNumber = "01000000001",
                    PhoneNumberConfirmed = true,
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
                    PhoneNumber = "01000000002",
                    PhoneNumberConfirmed = true,
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
                    PhoneNumber = "01000000003",
                    PhoneNumberConfirmed = true,
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
                    PhoneNumber = "01000000004",
                    PhoneNumberConfirmed = true,
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
                    PhoneNumber = "01000000005",
                    PhoneNumberConfirmed = true,
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
                    PhoneNumber = "01000000006",
                    PhoneNumberConfirmed = true,
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
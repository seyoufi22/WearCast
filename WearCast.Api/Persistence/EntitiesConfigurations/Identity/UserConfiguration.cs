using WearCast.Api.Entities.Identity;

namespace WearCast.Api.Persistence.EntitiesConfigurations.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
               .OwnsMany(x => x.RefreshTokens)
               .ToTable("RefreshTokens")
               .WithOwner()
               .HasForeignKey("UserId");

            builder.Property(x => x.FirstName).HasMaxLength(100);
            builder.Property(x => x.LastName).HasMaxLength(100);

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
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.SuperAdminPassword)
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
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.CustomerPassword)
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
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.SellerPassword)
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
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.FactoryPassword)
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
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.ShippingCompanyPassword)
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
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.DriverPassword)
                }

            };

            builder.HasData(seedUsers);
        }
    }
}

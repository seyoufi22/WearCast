using WearCast.Api.Entities.Identity;

namespace WearCast.Api.Persistence.EntitiesConfigurations.Identity
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            //builder.HasIndex(x => x.Title).IsUnique();
            //builder.Property(x => x.Title).HasMaxLength(100);
            //builder.Property(x => x.Summary).HasMaxLength(1500);

            var seedRoles = new List<ApplicationRole>
            {
                new ApplicationRole
                {
                    Id = DefaultRoles.SuperAdminRoleId,
                    Name = DefaultRoles.SuperAdmin,
                    NormalizedName = DefaultRoles.SuperAdmin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.SuperAdminRoleConcurrencyStamp
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.CustomerRoleId,
                    Name = DefaultRoles.Customer,
                    NormalizedName = DefaultRoles.Customer.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.CustomerRoleConcurrencyStamp
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.SellerRoleId,
                    Name = DefaultRoles.Seller,
                    NormalizedName = DefaultRoles.Seller.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.SellerRoleConcurrencyStamp
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.FactoryRoleId,
                    Name = DefaultRoles.Factory,
                    NormalizedName = DefaultRoles.Factory.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.FactoryRoleConcurrencyStamp
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.ShippingCompanyRoleId,
                    Name = DefaultRoles.ShippingCompany,
                    NormalizedName = DefaultRoles.ShippingCompany.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.ShippingCompanyRoleConcurrencyStamp
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.DriverRoleId,
                    Name = DefaultRoles.Driver,
                    NormalizedName = DefaultRoles.Driver.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.DriverRoleConcurrencyStamp
                },

            };

            builder.HasData(seedRoles);
        }
    }
}

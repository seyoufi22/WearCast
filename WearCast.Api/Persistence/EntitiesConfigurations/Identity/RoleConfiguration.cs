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
                    Id = DefaultRoles.OperationsAdminRoleId,
                    Name = DefaultRoles.OperationsAdmin,
                    NormalizedName = DefaultRoles.OperationsAdmin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.OperationsAdminRoleConcurrencyStamp
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.VendorAdminRoleId,
                    Name = DefaultRoles.VendorAdmin,
                    NormalizedName = DefaultRoles.VendorAdmin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.VendorAdminRoleConcurrencyStamp
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.CatalogAdminRoleId,
                    Name = DefaultRoles.CatalogAdmin,
                    NormalizedName = DefaultRoles.CatalogAdmin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.CatalogAdminRoleConcurrencyStamp
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.CustomerServiceAdminRoleId,
                    Name = DefaultRoles.CustomerServiceAdmin,
                    NormalizedName = DefaultRoles.CustomerServiceAdmin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.CustomerServiceAdminRoleConcurrencyStamp
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
                    Id = DefaultRoles.SellerManagerRoleId,
                    Name = DefaultRoles.SellerManager,
                    NormalizedName = DefaultRoles.SellerManager.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.SellerManagerRoleConcurrencyStamp
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.FactoryManagerRoleId,
                    Name = DefaultRoles.FactoryManager,
                    NormalizedName = DefaultRoles.FactoryManager.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.FactoryManagerRoleConcurrencyStamp
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.ShippingCompanyManagerRoleId,
                    Name = DefaultRoles.ShippingCompanyManager,
                    NormalizedName = DefaultRoles.ShippingCompanyManager.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.ShippingCompanyManagerRoleConcurrencyStamp
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

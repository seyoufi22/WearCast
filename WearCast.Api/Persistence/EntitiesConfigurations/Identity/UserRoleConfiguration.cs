namespace WearCast.Api.Persistence.EntitiesConfigurations.Identity
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {

            var seedUserRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    UserId = DefaultUsers.SuperAdminId,
                    RoleId = DefaultRoles.SuperAdminRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = DefaultUsers.CustomerId,
                    RoleId = DefaultRoles.CustomerRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = DefaultUsers.SellerId,
                    RoleId = DefaultRoles.SellerRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = DefaultUsers.FactoryId,
                    RoleId = DefaultRoles.FactoryRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = DefaultUsers.ShippingCompanyId,
                    RoleId = DefaultRoles.ShippingCompanyRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = DefaultUsers.DriverId,
                    RoleId = DefaultRoles.DriverRoleId
                },

            };
            builder.HasData(seedUserRoles);
        }
    }
}

namespace WearCast.Api.Persistence.EntitiesConfigurations.Identity
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            var claims = new List<IdentityRoleClaim<string>>();
            int idCounter = 1;

            var allPermissions = Permissions.GetAllPermissions();
            foreach (var permission in allPermissions)
            {
                claims.Add(CreateClaim(idCounter++, permission, DefaultRoles.SuperAdminRoleId));
            }

            foreach (var permission in Permissions.CustomerPermissions)
            {
                claims.Add(CreateClaim(idCounter++, permission, DefaultRoles.CustomerRoleId));
            }

            foreach (var permission in Permissions.SellerPermissions)
            {
                claims.Add(CreateClaim(idCounter++, permission, DefaultRoles.SellerRoleId));
            }

            foreach (var permission in Permissions.FactoryPermissions)
            {
                claims.Add(CreateClaim(idCounter++, permission, DefaultRoles.FactoryRoleId));
            }

            foreach (var permission in Permissions.ShippingCompanyPermissions)
            {
                claims.Add(CreateClaim(idCounter++, permission, DefaultRoles.ShippingCompanyRoleId));
            }

            foreach (var permission in Permissions.DriverPermissions)
            {
                claims.Add(CreateClaim(idCounter++, permission, DefaultRoles.DriverRoleId));
            }

            builder.HasData(claims);
        }

        private IdentityRoleClaim<string> CreateClaim(int id, string permission, string roleId)
        {
            return new IdentityRoleClaim<string>
            {
                Id = id,
                RoleId = roleId,
                ClaimType = Permissions.Type,
                ClaimValue = permission
            };
        }
    }
}
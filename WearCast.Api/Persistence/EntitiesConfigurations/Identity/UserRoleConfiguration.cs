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
                }
            };
            builder.HasData(seedUserRoles);
        }
    }
}

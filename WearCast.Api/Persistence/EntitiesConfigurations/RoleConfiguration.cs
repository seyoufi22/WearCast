
namespace WearCast.Api.Persistence.EntitiesConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData([
                new ApplicationRole
                {
                    Id = DefaultRoles.AdminRoleId,
                    Name = DefaultRoles.Admin,
                    NormalizedName = DefaultRoles.Admin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp,
                    IsDefault = false,
                    IsDeleted = false
                },
                new ApplicationRole
                {
                    Id = DefaultRoles.CustomerRoleId,
                    Name = DefaultRoles.Customer,
                    NormalizedName = DefaultRoles.Customer.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.CustomerRoleConcurrencyStamp,
                    IsDefault = true,
                    IsDeleted = false
                }
            ]);
        }
    }
}

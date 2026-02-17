using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WearCast.Api.Common.Consts;

namespace WearCast.Api.Persistence.EntitiesConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {

        var adminRoles = DefaultUsers.Admins.Select(a => new IdentityUserRole<string>
        {
            UserId = a.Id,
            RoleId = DefaultRoles.AdminRoleId
        }).ToArray();

        builder.HasData(adminRoles);

    }
}

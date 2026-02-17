using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WearCast.Api.Common.Consts;
using WearCast.Api.Entities;

namespace WearCast.Api.Persistence.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        //Default Data

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        var adminUsers = DefaultUsers.Admins.Select(a => new ApplicationUser
        {
            Id = a.Id,
            FirstName = "Admin",
            LastName = "User",
            UserName = a.Email,
            NormalizedUserName = a.Email.ToUpper(),
            Email = a.Email,
            NormalizedEmail = a.Email.ToUpper(),
            SecurityStamp = a.SecurityStamp,
            ConcurrencyStamp = a.ConcurrencyStamp,
            EmailConfirmed = true,
            PasswordHash = passwordHasher.HashPassword(null!, a.Password)
        }).ToArray();

        builder.HasData(adminUsers);
    }
}

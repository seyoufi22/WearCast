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
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.SuperAdminPassword)
                }
            };

            builder.HasData(seedUsers);
        }
    }
}
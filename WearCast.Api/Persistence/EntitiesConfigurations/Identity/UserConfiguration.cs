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

        }
    }
}
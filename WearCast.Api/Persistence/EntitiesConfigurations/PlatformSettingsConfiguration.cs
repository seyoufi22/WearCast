using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WearCast.Api.Entities;

namespace WearCast.Api.Persistence.EntitiesConfigurations;

public class PlatformSettingsConfiguration : IEntityTypeConfiguration<PlatformSettings>
{
    public void Configure(EntityTypeBuilder<PlatformSettings> builder)
    {
        // Enforce singleton pattern at the database level.
        // Only one row with Id = 1 can exist.
        builder.HasCheckConstraint("CK_PlatformSettings_Singleton", "[Id] = 1");

        builder.Property(p => p.CommissionPercentage)
            .HasPrecision(18, 2);

        // Seed the default platform settings
        builder.HasData(new PlatformSettings
        {
            Id = 1,
            CommissionPercentage = 2m,
            UpdatedOn = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}

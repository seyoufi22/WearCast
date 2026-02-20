using WearCast.Api.Entities.FixedProduct;

namespace WearCast.Api.Persistence.EntitiesConfigurations.FixedProductConfiguration;

public class FixedProductSizeConfiguration : IEntityTypeConfiguration<FixedProductSize>
{
    public void Configure(EntityTypeBuilder<FixedProductSize> builder)
    {
        builder.HasKey(s => new { s.ProductColorId, s.Size });

        builder.Property(s => s.Size)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasOne(s => s.ProductColor)
            .WithMany(c => c.Sizes)
            .HasForeignKey(s => s.ProductColorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}

using WearCast.Api.Entities.FixedProduct;

namespace WearCast.Api.Persistence.EntitiesConfigurations.FixedProductConfiguration;

public class FixedProductColorConfiguration : BaseModelConfiguration<FixedProductColor>
{
    public override void Configure(EntityTypeBuilder<FixedProductColor> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.ColorName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.ColorCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.ImageUrl)
            .IsRequired();

        builder.HasOne(c => c.Product)
            .WithMany(p => p.Colors)
            .HasForeignKey(c => c.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}

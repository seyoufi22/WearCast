using WearCast.Api.Entities.FixedProduct;

namespace WearCast.Api.Persistence.EntitiesConfigurations.FixedProductConfiguration;

public class FixedProductImageConfiguration : BaseModelConfiguration<FixedProductImage>
{
    public override void Configure(EntityTypeBuilder<FixedProductImage> builder)
    {
        base.Configure(builder);

        builder.Property(i => i.ImageUrl)
            .IsRequired();

        builder.HasOne(i => i.ProductColor)
            .WithMany(c => c.Images)
            .HasForeignKey(i => i.ProductColorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}

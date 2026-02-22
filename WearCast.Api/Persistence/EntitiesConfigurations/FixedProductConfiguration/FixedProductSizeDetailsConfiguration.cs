using WearCast.Api.Entities.FixedProduct;

namespace WearCast.Api.Persistence.EntitiesConfigurations.FixedProductConfiguration;

public class FixedProductSizeDetailsConfiguration : BaseModelConfiguration<FixedProductSizeDetails>
{
    public override void Configure(EntityTypeBuilder<FixedProductSizeDetails> builder)
    {
        base.Configure(builder);

        builder.Property(d => d.Size)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasOne(d => d.Product)
            .WithMany(p => p.SizeDetails)
            .HasForeignKey(d => d.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}

using WearCast.Api.Entities.DesignedProducts;

namespace WearCast.Api.Persistence.EntitiesConfigurations.DesignedProductsConfiguration
{
    public class DesignedProductConfiguation : BaseModelConfiguration<DesignedProduct>
    {
        public override void Configure(EntityTypeBuilder<DesignedProduct> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(200);

            builder.Property(x => x.Slug)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(x => x.Slug)
                   .IsUnique();

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.Property(x => x.TargetAudience)
                   .IsRequired()
                   .HasConversion<byte>();

            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.CanvasWidth).IsRequired();
            builder.Property(x => x.CanvasHeight).IsRequired();
        }
    }

}

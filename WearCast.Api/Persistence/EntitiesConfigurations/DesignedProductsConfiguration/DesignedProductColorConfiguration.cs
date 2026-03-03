using WearCast.Api.Entities.DesignedProducts;

namespace WearCast.Api.Persistence.EntitiesConfigurations.DesignedProductsConfiguration
{
    public class DesignedProductColorConfiguration : BaseModelConfiguration<DesignedProductColor>
    {
        public override void Configure(EntityTypeBuilder<DesignedProductColor> builder)
        {
            builder.Property(x => x.Name)
                       .IsRequired()
                       .HasMaxLength(100);

            builder.Property(x => x.HexCode)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(x => x.Slug)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.DesignedProductId)
                  .IsRequired();

            builder.HasIndex(x => new { x.DesignedProductId, x.Slug })
                   .IsUnique();

            builder.HasOne(x => x.DesignedProduct)
                   .WithMany(p => p.Colors)
                   .HasForeignKey(x => x.DesignedProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using WearCast.Api.Entities.DesignedProducts;

namespace WearCast.Api.Persistence.EntitiesConfigurations.DesignedProductsConfiguration
{
    public class DesignAssetCategoryConfiguration : BaseModelConfiguration<DesignAssetCategory>
    {
        public override void Configure(EntityTypeBuilder<DesignAssetCategory> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(100);

            builder.HasIndex(x => x.Slug).IsUnique();
        }
    }
}

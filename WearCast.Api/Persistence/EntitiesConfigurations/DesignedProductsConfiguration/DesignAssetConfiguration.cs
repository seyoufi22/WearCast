namespace WearCast.Api.Persistence.EntitiesConfigurations.DesignedProductsConfiguration
{
    public class DesignAssetConfiguration : BaseModelConfiguration<DesignAsset>
    {
        public override void Configure(EntityTypeBuilder<DesignAsset> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(1000);

            //builder.Property(x => x.WidthPx)
            //       .IsRequired();

            //builder.Property(x => x.HeightPx)
            //       .IsRequired();

            builder.HasOne(x => x.DesignAssetCategory)
                   .WithMany(c => c.Assets)
                   .HasForeignKey(x => x.DesignAssetCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

using WearCast.Api.Entities.DesignedProducts;

namespace WearCast.Api.Persistence.EntitiesConfigurations.DesignedProductsConfiguration
{
    public class DesignedProductImageConfiguration : BaseModelConfiguration<DesignedProductImage>
    {
        public override void Configure(EntityTypeBuilder<DesignedProductImage> builder)
        {
            builder.Property(x => x.ImageUrl)
                   .IsRequired();

            builder.Property(x => x.ViewSide)
                   .IsRequired()
                   .HasConversion<byte>();

            builder.HasOne(x => x.DesignedProductColor)
                   .WithMany(c => c.Images)
                   .HasForeignKey(x => x.DesignedProductColorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
}

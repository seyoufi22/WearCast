using WearCast.Api.Entities.DesignedProducts;

namespace WearCast.Api.Persistence.EntitiesConfigurations.DesignedProductsConfiguration
{
    public class CustomerUploadedImageConfiguration : BaseModelConfiguration<CustomerUploadedImage>
    {
        public override void Configure(EntityTypeBuilder<CustomerUploadedImage> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.ImageUrl).IsRequired();

            builder.HasOne(x => x.Customer)
                   .WithMany()
                   .HasForeignKey(x => x.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

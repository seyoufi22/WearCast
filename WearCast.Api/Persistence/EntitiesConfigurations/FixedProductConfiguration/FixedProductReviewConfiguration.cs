using WearCast.Api.Entities.FixedProduct;

namespace WearCast.Api.Persistence.EntitiesConfigurations.FixedProductsConfiguration
{
    public class FixedProductReviewConfiguration : IEntityTypeConfiguration<FixedProductReview>
    {
        public void Configure(EntityTypeBuilder<FixedProductReview> builder)
        {
            builder.HasIndex(r => new { r.FixedProductId, r.CustomerId })
                   .IsUnique()
                   .HasFilter("[IsDeleted] = 0"); 

            builder.HasOne(r => r.Customer)
                   .WithMany()
                   .HasForeignKey(r => r.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.FixedProduct)
                   .WithMany(p => p.Reviews)
                   .HasForeignKey(r => r.FixedProductId)
                   .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
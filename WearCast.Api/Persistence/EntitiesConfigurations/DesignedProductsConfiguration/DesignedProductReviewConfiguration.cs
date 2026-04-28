namespace WearCast.Api.Persistence.EntitiesConfigurations.DesignedProductsConfiguration
{
    public class DesignedProductReviewConfiguration : IEntityTypeConfiguration<DesignedProductReview>
    {
        public void Configure(EntityTypeBuilder<DesignedProductReview> builder)
        {
            builder.HasIndex(r => new { r.DesignedProductId, r.CustomerId })
                   .IsUnique()
                   .HasFilter("[IsDeleted] = 0");

            builder.HasOne(r => r.Customer)
                   .WithMany()
                   .HasForeignKey(r => r.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.DesignedProduct)
                   .WithMany()
                   .HasForeignKey(r => r.DesignedProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

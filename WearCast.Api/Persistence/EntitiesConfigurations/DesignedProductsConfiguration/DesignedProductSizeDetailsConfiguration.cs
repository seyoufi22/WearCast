namespace WearCast.Api.Persistence.EntitiesConfigurations.DesignedProductsConfiguration
{
    public class DesignedProductSizeDetailsConfiguration : IEntityTypeConfiguration<DesignedProductSizeDetails>
    {
        public void Configure(EntityTypeBuilder<DesignedProductSizeDetails> builder)
        {
            builder.HasIndex(x => new { x.DesignedProductId, x.Size })
                   .IsUnique()
                   .HasFilter("[IsDeleted] = 0");

            builder.HasOne(x => x.DesignedProduct)
                   .WithMany(x => x.SizeDetails)
                   .HasForeignKey(x => x.DesignedProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

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

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.Property(x => x.TargetAudience)
                   .IsRequired()
                   .HasConversion<byte>();

            builder.Property(x => x.Price)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Factory)
                   .WithMany(f => f.DesignedProducts)
                   .HasForeignKey(x => x.FactoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                   .WithMany(c => c.DesignedProducts)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}

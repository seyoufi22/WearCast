namespace WearCast.Api.Persistence.EntitiesConfigurations.DesignedProductsConfiguration
{
    public class CustomerDesignConfiguration : BaseModelConfiguration<CustomerDesign>
    {
        public override void Configure(EntityTypeBuilder<CustomerDesign> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.ViewDesignsJson)
                   .IsRequired();


            builder.HasOne(x => x.DesignedProduct)
                   .WithMany()
                   .HasForeignKey(x => x.DesignedProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DesignedProductColor)
                   .WithMany()
                   .HasForeignKey(x => x.DesignedProductColorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Customer)
                   .WithMany(c => c.Designs)
                   .HasForeignKey(x => x.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

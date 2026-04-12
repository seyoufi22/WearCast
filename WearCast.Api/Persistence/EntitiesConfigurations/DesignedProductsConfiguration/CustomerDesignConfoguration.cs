namespace WearCast.Api.Persistence.EntitiesConfigurations.DesignedProductsConfiguration
{
    public class CustomerDesignConfiguration : BaseModelConfiguration<CustomerDesign>
    {
        public override void Configure(EntityTypeBuilder<CustomerDesign> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.ViewDesignsJson)
                   .IsRequired();

<<<<<<< HEAD
=======
            builder.Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");
>>>>>>> 77b3b6565bae540371f7321998414a8d9608eab3

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

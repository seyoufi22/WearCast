namespace WearCast.Api.Persistence.Configurations.DesignedProducts
{
    public class DesignedProductSizeDetailsConfiguration : IEntityTypeConfiguration<DesignedProductSizeDetails>
    {
        public void Configure(EntityTypeBuilder<DesignedProductSizeDetails> builder)
        {
            builder.HasKey(x => x.Id);


            builder.Property(x => x.A).HasPrecision(8, 2);
            builder.Property(x => x.B).HasPrecision(8, 2);
            builder.Property(x => x.C).HasPrecision(8, 2);


            builder.Property(x => x.Size)
                     .IsRequired()
                     .HasConversion<byte>();

            builder.HasOne(x => x.DesignedProduct)
                .WithMany(p => p.SizeDetails)
                .HasForeignKey(x => x.DesignedProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
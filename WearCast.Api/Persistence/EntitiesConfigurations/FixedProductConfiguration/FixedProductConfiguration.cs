namespace WearCast.Api.Persistence.EntitiesConfigurations.FixedProductConfiguration;

public class FixedProductConfiguration : BaseModelConfiguration<Entities.FixedProduct.FixedProduct>
{
    public override void Configure(EntityTypeBuilder<Entities.FixedProduct.FixedProduct> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.TargetAudience)
            .IsRequired();

        builder.Property(p => p.DressStyle)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Seller)
            .WithMany(s => s.FixedProducts)
            .HasForeignKey(p => p.SellerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsMany(p => p.SizeDetails, a =>
        {
            a.ToJson();
            
            a.Property(sd => sd.Size)
                .IsRequired()
                .HasConversion<string>();

            a.Property(sd => sd.A).HasColumnType("decimal(18,2)");
            a.Property(sd => sd.B).HasColumnType("decimal(18,2)");
            a.Property(sd => sd.C).HasColumnType("decimal(18,2)");
        });
    }
}

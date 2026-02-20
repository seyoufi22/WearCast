namespace WearCast.Api.Persistence.EntitiesConfigurations.FixedProductConfiguration;

public class FixedProductConfiguration : BaseModelConfiguration<Entities.FixedProduct.FixedProduct>
{
    public void Configure(EntityTypeBuilder<Entities.FixedProduct.FixedProduct> builder)
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
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}

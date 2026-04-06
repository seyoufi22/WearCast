using WearCast.Api.Entities.Order;

namespace WearCast.Api.Persistence.EntitiesConfigurations.OrderConfiguration;

public class FixedProductOrderItemConfiguration : IEntityTypeConfiguration<FixedProductOrderItem>
{
    public void Configure(EntityTypeBuilder<FixedProductOrderItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.ColorName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.SizeName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(i => i.FixedColor)
            .WithMany()
            .HasForeignKey(i => i.FixedColorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

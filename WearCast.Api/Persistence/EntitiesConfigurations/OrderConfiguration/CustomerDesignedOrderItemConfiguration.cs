using WearCast.Api.Entities.Order;

namespace WearCast.Api.Persistence.EntitiesConfigurations.OrderConfiguration;

public class CustomerDesignedOrderItemConfiguration : IEntityTypeConfiguration<CustomerDesignedOrderItem>
{
    public void Configure(EntityTypeBuilder<CustomerDesignedOrderItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.ColorName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.SizeName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.FrontImageUrl)
            .HasMaxLength(500);

        builder.Property(i => i.BackImageUrl)
            .HasMaxLength(500);

        builder.Property(i => i.RightImageUrl)
            .HasMaxLength(500);

        builder.Property(i => i.LeftImageUrl)
            .HasMaxLength(500);
    }
}

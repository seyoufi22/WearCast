using WearCast.Api.Entities.Order;

namespace WearCast.Api.Persistence.EntitiesConfigurations.OrderConfiguration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(o => o.StripeSessionId)
            .HasMaxLength(500);

        builder.Property(o => o.StripePaymentIntentId)
            .HasMaxLength(500);

        builder.Property(o => o.RecipientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.RecipientPhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(o => o.RecipientAdditionalPhoneNumber)
            .HasMaxLength(20);

        builder.OwnsOne(o => o.ShippingAddress);

        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Seller)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.FixedProductItems)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


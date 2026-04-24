using WearCast.Api.Entities.Shipping;

namespace WearCast.Api.Persistence.EntitiesConfigurations.ShippingConfigurations
{
    public class ShipmentConfiguration: BaseModelConfiguration<Shipment>
    {
        public override void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasOne(s => s.Driver)
                     .WithMany(d => d.Shipments)
                     .HasForeignKey(s => s.DriverId)
                     .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.ShippingCompany)
                   .WithMany(c => c.Shipments)
                   .HasForeignKey(s => s.ShippingCompanyId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Customer)
                   .WithMany(c => c.Shipments)
                   .HasForeignKey(s => s.CustomerId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(s => s.DeliveryAddress);
        }
    }
}

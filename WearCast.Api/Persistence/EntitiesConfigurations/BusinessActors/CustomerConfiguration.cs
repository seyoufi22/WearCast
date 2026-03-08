
namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasOne(c => c.ApplicationUser)
                 .WithOne()
                 .HasForeignKey<Customer>(c => c.UserId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.State).IsRequired().HasMaxLength(50).HasColumnName("State");
                address.Property(a => a.City).IsRequired().HasMaxLength(50).HasColumnName("City");
                address.Property(a => a.Street).IsRequired().HasMaxLength(200).HasColumnName("Street");
                address.Property(a => a.BuildingNumber).IsRequired().HasMaxLength(20).HasColumnName("BuildingNumber");
            });
        }
    }
}

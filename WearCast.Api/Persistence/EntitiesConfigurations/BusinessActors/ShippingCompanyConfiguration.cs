
namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class ShippingCompanyConfiguration : IEntityTypeConfiguration<ShippingCompany>
    {
        public void Configure(EntityTypeBuilder<ShippingCompany> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasOne(sh => sh.ApplicationUser)
                 .WithOne()
                 .HasForeignKey<ShippingCompany>(sh => sh.UserId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

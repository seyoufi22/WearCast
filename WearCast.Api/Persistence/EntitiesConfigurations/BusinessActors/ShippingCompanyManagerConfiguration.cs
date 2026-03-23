namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class ShippingCompanyManagerConfiguration : IEntityTypeConfiguration<ShippingCompanyManager>
    {
        public void Configure(EntityTypeBuilder<ShippingCompanyManager> builder)
        {

            builder.HasOne(scm => scm.ApplicationUser)
                   .WithOne(u => u.ShippingCompanyManager)
                   .HasForeignKey<ShippingCompanyManager>(scm => scm.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(shm => shm.ShippingCompany)
                   .WithMany(sh => sh.Managers)
                   .HasForeignKey(shm => shm.ShippingCompanyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

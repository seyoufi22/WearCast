namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class ShippingCompanyManagerConfiguration : IEntityTypeConfiguration<ShippingCompanyManager>
    {
        public void Configure(EntityTypeBuilder<ShippingCompanyManager> builder)
        {
            builder.HasOne(shm => shm.ApplicationUser)
                 .WithMany()
                 .HasForeignKey(shm => shm.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(shm => shm.ShippingCompany)
                   .WithMany(sh => sh.Managers)
                   .HasForeignKey(shm => shm.ShippingCompanyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

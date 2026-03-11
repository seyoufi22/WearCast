namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors.Sellers
{
    public class SellerManagerConfiguration : IEntityTypeConfiguration<SellerManager>
    {
        public void Configure(EntityTypeBuilder<SellerManager> builder)
        {
            builder.HasOne(sm => sm.ApplicationUser)
                  .WithOne(u => u.SellerManager)
                  .HasForeignKey<SellerManager>(sm => sm.UserId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sm => sm.Seller)
                   .WithMany(s => s.Managers)
                   .HasForeignKey(sm => sm.SellerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

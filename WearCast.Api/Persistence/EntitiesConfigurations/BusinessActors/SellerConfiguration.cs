
namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.HasOne(s => s.ApplicationUser)
                 .WithOne()
                 .HasForeignKey<Seller>(s => s.UserId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

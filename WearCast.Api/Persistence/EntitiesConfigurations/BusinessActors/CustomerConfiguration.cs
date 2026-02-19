
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
        }
    }
}

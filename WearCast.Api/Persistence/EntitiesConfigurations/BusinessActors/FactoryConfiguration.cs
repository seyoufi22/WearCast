
namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class FactoryConfiguration : IEntityTypeConfiguration<Factory>
    {
        public void Configure(EntityTypeBuilder<Factory> builder)
        {
            builder.HasOne(f => f.ApplicationUser)
                 .WithOne()
                 .HasForeignKey<Factory>(f => f.UserId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

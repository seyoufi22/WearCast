namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class FactoryManagerConfiguration : IEntityTypeConfiguration<FactoryManager>
    {
        public void Configure(EntityTypeBuilder<FactoryManager> builder)
        {
            builder.HasOne(fm => fm.ApplicationUser)
                  .WithMany()
                  .HasForeignKey(fm => fm.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fm => fm.Factory)
                   .WithMany(f => f.Managers)
                   .HasForeignKey(fm => fm.FactoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

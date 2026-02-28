namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class FactoryManagerConfiguration : IEntityTypeConfiguration<FactoryManager>
    {
        public void Configure(EntityTypeBuilder<FactoryManager> builder)
        {
            builder.HasOne(sm => sm.ApplicationUser)
                  .WithMany()
                  .HasForeignKey(sm => sm.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fm => fm.Factory)
                   .WithMany(s => s.Managers)
                   .HasForeignKey(fm => fm.FactoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

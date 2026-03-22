namespace WearCast.Api.Persistence.EntitiesConfigurations.BusinessActors
{
    public class FactoryManagerConfiguration : IEntityTypeConfiguration<FactoryManager>
    {
        public void Configure(EntityTypeBuilder<FactoryManager> builder)
        {
            builder.HasOne(fm => fm.ApplicationUser)
                  .WithOne(u => u.FactoryManager)
                  .HasForeignKey<FactoryManager>(fm => fm.UserId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fm => fm.Factory)
                   .WithMany(f => f.Managers)
                   .HasForeignKey(fm => fm.FactoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

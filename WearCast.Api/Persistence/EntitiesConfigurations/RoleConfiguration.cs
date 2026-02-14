
namespace WearCast.Api.Persistence.EntitiesConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            //builder.HasIndex(x => x.Title).IsUnique();
            //builder.Property(x => x.Title).HasMaxLength(100);
            //builder.Property(x => x.Summary).HasMaxLength(1500);
        }
    }
}

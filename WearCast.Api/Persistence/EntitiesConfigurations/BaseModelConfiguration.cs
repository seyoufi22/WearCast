namespace WearCast.Api.Persistence.EntitiesConfigurations
{
    public class BaseModelConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseModel
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // 1. تحديد الـ Primary Key
            builder.HasKey(e => e.ID);

            // 2. إعداد الـ Guid Identity
            builder.Property(e => e.ID)
                   .HasDefaultValueSql("NEWID()")
                   .ValueGeneratedOnAdd();

        }
    }
}

namespace WearCast.Api.Persistence.EntitiesConfigurations
{
    public class BaseModelConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseModel
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {

        }
    }
}
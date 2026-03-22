namespace WearCast.Api.Persistence.EntitiesConfigurations
{
    public class BaseModelConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseModel
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);

            //  builder.Property(x => x.CreatedById).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();

            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder.HasOne(x => x.CreatedBy)
                   .WithMany()
                   .HasForeignKey(x => x.CreatedById)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UpdatedBy)
                   .WithMany()
                   .HasForeignKey(x => x.UpdatedById)
                   .OnDelete(DeleteBehavior.Restrict);

            //  builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
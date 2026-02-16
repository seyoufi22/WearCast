namespace WearCast.Api.Persistence.EntitiesConfigurations
{
    public class CategoryConfiguration : BaseModelConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.ImageUrl)
                .IsRequired();
        }
    }
}

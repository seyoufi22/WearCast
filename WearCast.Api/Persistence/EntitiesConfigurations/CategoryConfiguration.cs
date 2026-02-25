namespace WearCast.Api.Persistence.EntitiesConfigurations
{
    public class CategoryConfiguration : BaseModelConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(c => c.ImageUrl)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}

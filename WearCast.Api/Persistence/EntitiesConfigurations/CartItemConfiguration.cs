namespace WearCast.Api.Persistence.EntitiesConfigurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(x => new { x.ColorId, x.CustomerId });

        builder.HasOne(x => x.Customer)
               .WithMany(x => x.CartItems)
               .HasForeignKey(x => x.CustomerId);

        builder.HasOne(x => x.Color)
           .WithMany(x => x.CartItems)
           .HasForeignKey(x => x.ColorId).
           OnDelete(DeleteBehavior.Restrict);

        builder.OwnsMany(c => c.Sizes, a =>
        {
            a.ToJson();

            a.Property(s => s.Size)
                .IsRequired()
                .HasConversion<string>();
        });
    }
}

namespace WearCast.Api.Persistence.EntitiesConfigurations;

public class CartItemConfiguration : BaseModelConfiguration<CartItem>
{
    public override void Configure(EntityTypeBuilder<CartItem> builder)
    {
        base.Configure(builder);

        builder.HasIndex(c => new { c.CustomerId, c.FixedColorId });
        builder.HasIndex(c => new { c.CustomerDesignId, c.CustomerId });

        builder.HasOne(x => x.Customer)
               .WithMany(x => x.CartItems)
               .HasForeignKey(x => x.CustomerId);

        builder.HasOne(x => x.FixedColor)
           .WithMany(x => x.CartItems)
           .HasForeignKey(x => x.FixedColorId).
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

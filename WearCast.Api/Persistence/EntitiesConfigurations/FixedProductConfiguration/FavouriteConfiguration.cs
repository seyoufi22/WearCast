using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WearCast.Api.Entities.FixedProduct;

namespace WearCast.Api.Persistence.EntitiesConfigurations.FixedProductConfiguration;

public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
{
    public void Configure(EntityTypeBuilder<Favourite> builder)
    {
        builder.HasKey(f => new { f.CustomerId, f.FixedProductColorId });

        builder.HasOne(f => f.Customer)
            .WithMany(c => c.Favourites)
            .HasForeignKey(f => f.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.FixedProductColor)
            .WithMany(fc => fc.Favourites)
            .HasForeignKey(f => f.FixedProductColorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

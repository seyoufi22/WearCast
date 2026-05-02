using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WearCast.Api.Entities.Wallet;

namespace WearCast.Api.Persistence.EntitiesConfigurations.WalletConfigurations;

public class WalletConfiguration : BaseModelConfiguration<Wallet>
{
    public override void Configure(EntityTypeBuilder<Wallet> builder)
    {
        base.Configure(builder);
    }
}

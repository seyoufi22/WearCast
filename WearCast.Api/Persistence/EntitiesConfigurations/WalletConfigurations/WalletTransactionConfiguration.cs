using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WearCast.Api.Entities.Wallet;

namespace WearCast.Api.Persistence.EntitiesConfigurations.WalletConfigurations;

public class WalletTransactionConfiguration : BaseModelConfiguration<WalletTransaction>
{
    public override void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        base.Configure(builder);

        // Also restrict cascade delete from Wallet -> WalletTransaction if needed,
        // or just let it cascade since BaseModel rules out the multiple cascade paths from ApplicationUser.
        // We will explicitly set it to Cascade from Wallet, which is fine since User->Wallet is Restrict.
        builder.HasOne(t => t.Wallet)
            .WithMany(w => w.Transactions)
            .HasForeignKey(t => t.WalletId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

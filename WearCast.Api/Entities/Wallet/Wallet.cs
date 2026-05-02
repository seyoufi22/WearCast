using WearCast.Api.Common.Enums;

namespace WearCast.Api.Entities.Wallet;

public class Wallet : BaseModel
{
    public decimal Balance { get; set; } = 0m;
    public WalletOwnerType OwnerType { get; set; }
    public int OwnerId { get; set; }

    public ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
}

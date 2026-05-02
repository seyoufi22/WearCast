using WearCast.Api.Common.Enums;

namespace WearCast.Api.Entities.Wallet;

public class WalletTransaction : BaseModel
{
    public int WalletId { get; set; }
    public Wallet Wallet { get; set; } = null!;

    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? ReferenceOrderId { get; set; }
}

public enum TransactionType
{
    Credit = 1,
    Debit = 2
}

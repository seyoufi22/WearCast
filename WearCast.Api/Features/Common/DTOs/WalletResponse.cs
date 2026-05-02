namespace WearCast.Api.Features.Common.DTOs;

public record WalletResponse(
    int WalletId,
    decimal Balance,
    List<WalletTransactionDto> RecentTransactions
);

public record WalletTransactionDto(
    int Id,
    string Type,
    decimal Amount,
    decimal BalanceAfter,
    string Description,
    int? ReferenceOrderId,
    string? SenderName,
    string? SenderEmail,
    DateTime CreatedOn
);

using WearCast.Api.Features.CartManagment.DeleteCartItem.DTOs;

namespace WearCast.Api.Features.CartManagment.DeleteCartItem;

public class DeleteCartItemHandler(IRepository<CartItem> _cartItemRepository)
    : IRequestHandler<DeleteCartItemCommand, Result>
{
    public async Task<Result> Handle(DeleteCartItemCommand command, CancellationToken cancellationToken)
    {
        var cartItem = await _cartItemRepository.GetAsync(
            c => c.Id == command.CartItemId);

        if (cartItem is null)
            return Result.Failure(new Error("Cart.NotFound", "Item not found", 404));

        if (cartItem.CustomerId != command.CustomerId)
            return Result.Failure(AuthErrors.Forbidden);

        await _cartItemRepository.HardDeleteAsync(cartItem);

        return Result.Success();
    }
}
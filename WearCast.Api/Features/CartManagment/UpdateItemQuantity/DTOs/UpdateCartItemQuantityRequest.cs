
namespace WearCast.Api.Features.CartManagment.UpdateCartItemQuantity.DTOs;

public record UpdateCartItemQuantityRequest(
    int CartItemId,
    Size Size,
    int NewQuantity
) : IRequest<Result>;
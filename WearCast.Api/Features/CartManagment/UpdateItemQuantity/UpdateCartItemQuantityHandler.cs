using System.Security.Claims;
namespace WearCast.Api.Features.CartManagment.UpdateCartItemQuantity;

using System.Drawing;
using WearCast.Api.Features.CartManagment.UpdateCartItemQuantity.DTOs;
public class UpdateCartItemQuantityHandler(
    IRepository<CartItem> _cartItemRepository,
    ApplicationDbContext _dbContext,
    IHttpContextAccessor _httpContextAccessor)
    : IRequestHandler<UpdateCartItemQuantityRequest, Result>
{
    public async Task<Result> Handle(UpdateCartItemQuantityRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue("CustomerId");
        if (string.IsNullOrEmpty(userIdClaim))
            return Result.Failure(new Error("Auth.Unauthorized", "User not identified.", 401));

        var customerId = int.Parse(userIdClaim);

        var cartItem = await _cartItemRepository.Get()
            .Include(c => c.Sizes)
            .FirstOrDefaultAsync(c => c.Id == request.CartItemId &&
                                     c.CustomerId == customerId, cancellationToken);

        if (cartItem == null)
            return Result.Failure(new Error("Cart.NotFound", "The specified item was not found.", 404));
        var existingSize = cartItem.Sizes.FirstOrDefault(s => s.Size == request.Size);

        if (existingSize == null)
            return Result.Failure(new Error("CartItemSize.NotFound", "This size was not found in your cart for this item.", 404));
        
        if (request.NewQuantity <= 0)
        {
            cartItem.RemoveSize(request.Size);
        }
        else
        {
            existingSize.Quantity = request.NewQuantity;
        }

        if (!cartItem.Sizes.Any())
        {
            await _cartItemRepository.HardDeleteAsync(cartItem);
        }
        else
        {
            _dbContext.Entry(cartItem).State = EntityState.Modified; 
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}
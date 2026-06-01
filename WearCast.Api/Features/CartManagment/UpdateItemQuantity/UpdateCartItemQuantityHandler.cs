using System.Security.Claims;
using WearCast.Api.Features.CartManagment.UpdateCartItemQuantity.DTOs;

namespace WearCast.Api.Features.CartManagment.UpdateCartItemQuantity;

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

        // Fetch CartItem and Include FixedColor to check stock availability
        var cartItem = await _cartItemRepository.Get()
            .Include(c => c.Sizes)
            .Include(c => c.FixedColor)
                .ThenInclude(fc => fc!.Sizes)
            .FirstOrDefaultAsync(c => c.Id == request.CartItemId &&
                                     c.CustomerId == customerId, cancellationToken);

        if (cartItem == null)
            return Result.Failure(new Error("Cart.NotFound", "The specified item was not found.", 404));

        var existingSize = cartItem.Sizes.FirstOrDefault(s => s.Size == request.Size);

        if (existingSize == null)
            return Result.Failure(new Error("CartItemSize.NotFound", "This size was not found in your cart for this item.", 404));

        // 1. Calculate the new quantity by adding the change (+1 or -1)
        int newQuantity = existingSize.Quantity + request.QuantityChange;

        // 2. Check stock availability (Only if it's a fixed product and we are incrementing)
        if (request.QuantityChange > 0 && cartItem.FixedColorId != null)
        {
            var availableStock = cartItem.FixedColor!.Sizes.FirstOrDefault(s => s.Size == request.Size)?.Quantity ?? 0;
            if (newQuantity > availableStock)
            {
                return Result.Failure(new Error("Cart.StockExceeded", $"Only {availableStock} item(s) available in stock.", 400));
            }
        }

        // 3. Apply changes
        if (newQuantity <= 0)
        {
            cartItem.RemoveSize(request.Size);
        }
        else
        {
            existingSize.Quantity = newQuantity;
        }

        // 4. Save to Database
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
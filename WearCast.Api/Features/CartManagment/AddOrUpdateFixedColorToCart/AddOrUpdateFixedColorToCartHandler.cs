using WearCast.Api.Features.CartManagment.AddOrUpdateFixedColorToCart.DTOs;
using static WearCast.Api.Entities.CartItem;

namespace WearCast.Api.Features.CartManagment.AddOrUpdateFixedColorToCart;

public class AddOrUpdateFixedColorToCartHandler(
    IRepository<CartItem> _cartItemRepository,
    IRepository<Entities.FixedProduct.FixedProductColor> _colorRepository,
    ApplicationDbContext _dbContext)
    : IRequestHandler<AddOrUpdateFixedColorToCartCommand, Result>
{
    public async Task<Result> Handle(AddOrUpdateFixedColorToCartCommand command, CancellationToken cancellationToken)
    {
        var color = await _colorRepository.Get()
         .Include(c => c.Sizes)
         .FirstOrDefaultAsync(c => c.Id == command.Request.ColorId, cancellationToken);

        if (color == null)
            return Result.Failure(new Error("Color.NotFound", "The specified color does not exist.", 404));

        if (color.IsDeleted)
            return Result.Failure(new Error("Color.Unavailable", "The selected color is no longer available.", 400));

        var requestedSizes = command.Request.Sizes.Select(s => s.Size).ToList();
        var availableSizes = color.Sizes.Select(s => s.Size).ToList();

        var unavailableSizes = requestedSizes.Except(availableSizes).ToList();
        if (unavailableSizes.Any())
        {
            var missingSizesStr = string.Join(", ", unavailableSizes);
            return Result.Failure(new Error("Color.SizeUnavailable", $"The following sizes are not available for this product template: {missingSizesStr}", 400));
        }

        var cartItem = await _cartItemRepository.Get()
        .Include(c => c.Sizes)
        .FirstOrDefaultAsync(c =>
            c.FixedColorId == command.Request.ColorId &&
            c.CustomerId == command.CustomerId,
            cancellationToken);

        if (cartItem == null)
        {
            var validInitialSizes = command.Request.Sizes.Where(s => s.Quantity > 0).ToList();

            if (!validInitialSizes.Any())
                return Result.Success();

            cartItem = new CartItem
            {
                CustomerId = command.CustomerId,
                FixedColorId = command.Request.ColorId
            };

            var updates = validInitialSizes.Select(req =>
            {
                var stock = color.Sizes.First(s => s.Size == req.Size).Quantity;
                return new SizeUpdateDto(req.Size, req.Quantity, stock);
            }).ToList();

            // Handle the Result instead of catching exceptions
            var result = cartItem.AddOrUpdateSizes(updates);
            if (result.IsFailure)
            {
                return result;
            }

            await _cartItemRepository.CreateAsync(cartItem);
        }
        else
        {
            var updates = command.Request.Sizes.Select(req =>
            {
                var stock = color.Sizes.First(s => s.Size == req.Size).Quantity;
                return new SizeUpdateDto(req.Size, req.Quantity, stock);
            }).ToList();

            // Handle the Result instead of catching exceptions
            var result = cartItem.AddOrUpdateSizes(updates);
            if (result.IsFailure)
            {
                return result;
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
        }

        return Result.Success();
    }
}
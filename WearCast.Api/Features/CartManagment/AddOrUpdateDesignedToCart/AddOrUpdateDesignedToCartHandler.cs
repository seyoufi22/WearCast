using WearCast.Api.Features.CartManagment.AddOrUpdateDesignedToCart.DTOs;
using static WearCast.Api.Entities.CartItem;
namespace WearCast.Api.Features.CartManagment.AddOrUpdateDesignedColorToCart;

public class AddOrUpdateDesignedToCartHandler(IRepository<CartItem> _cartItemRepository, IRepository<CustomerDesign> _designedRepository)
    : IRequestHandler<AddOrUpdateDesignedToCartCommand, Result>
{
    public async Task<Result> Handle(AddOrUpdateDesignedToCartCommand command, CancellationToken cancellationToken)
    {
        // 1. Fetch the design, including the DesignedProductColor
        var design = await _designedRepository.Get()
            .Include(c => c.DesignedProduct)
                .ThenInclude(c => c.SizeDetails)
            .Include(c => c.DesignedProductColor) // 👈 Added: Include the linked color
            .FirstOrDefaultAsync(c =>
                c.Id == command.Request.DesignId &&
                !c.IsDeleted, cancellationToken);

        if (design == null)
            return Result.Failure(new Error("Design.NotFound", "The specified Design does not exist.", 404));

        // 2. 👈 Added: Check if the associated color is deleted
        // Note: Ensure the navigation property name matches your actual entity (e.g., DesignedProductColor)
        if (design.DesignedProductColor != null && design.DesignedProductColor.IsDeleted)
        {
            return Result.Failure(new Error("Design.ColorUnavailable", "The selected color for this product is no longer available.", 400));
        }

        if (design.CustomerId != command.CustomerId)
            return Result.Failure(AuthErrors.Forbidden);

        var requestedSizes = command.Request.Sizes.Select(s => s.Size).ToList();
        var availableSizes = design.DesignedProduct.SizeDetails.Select(s => s.Size).ToList();

        var unavailableSizes = requestedSizes.Except(availableSizes).ToList();
        if (unavailableSizes.Any())
        {
            var missingSizesStr = string.Join(", ", unavailableSizes);
            return Result.Failure(new Error("Design.SizeUnavailable", $"The following sizes are not available for this product: {missingSizesStr}", 400));
        }

        var cartItem = await _cartItemRepository.Get()
            .Include(c => c.Sizes)
            .FirstOrDefaultAsync(c =>
                c.CustomerDesignId == command.Request.DesignId &&
                c.CustomerId == command.CustomerId,
                cancellationToken);

        try
        {
            if (cartItem == null)
            {
                var validInitialSizes = command.Request.Sizes.Where(s => s.Quantity > 0).ToList();

                if (!validInitialSizes.Any())
                    return Result.Success();

                cartItem = new CartItem
                {
                    CustomerId = command.CustomerId,
                    CustomerDesignId = command.Request.DesignId
                };

                var updates = validInitialSizes.Select(item =>
                    new SizeUpdateDto(item.Size, item.Quantity, null)
                ).ToList();

                cartItem.AddOrUpdateSizes(updates);

                await _cartItemRepository.CreateAsync(cartItem);
            }
            else
            {
                var updates = command.Request.Sizes.Select(item =>
                    new SizeUpdateDto(item.Size, item.Quantity, null)
                ).ToList();

                cartItem.AddOrUpdateSizes(updates);

                if (!cartItem.Sizes.Any())
                {
                    await _cartItemRepository.HardDeleteAsync(cartItem);
                }
                else
                {
                    await _cartItemRepository.UpdateAsync(cartItem);
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(new Error("Cart.OperationFailed", ex.Message, 400));
        }

        return Result.Success();
    }
}
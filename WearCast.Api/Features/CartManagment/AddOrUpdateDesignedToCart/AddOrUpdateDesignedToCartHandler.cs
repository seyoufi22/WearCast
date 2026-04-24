using WearCast.Api.Features.CartManagment.AddOrUpdateDesignedToCart.DTOs;
namespace WearCast.Api.Features.CartManagment.AddOrUpdateDesignedColorToCart;

public class AddOrUpdateDesignedToCartHandler(IRepository<CartItem> _cartItemRepository, IRepository<CustomerDesign> _designedRepository)
    : IRequestHandler<AddOrUpdateDesignedToCartCommand, Result>
{
    public async Task<Result> Handle(AddOrUpdateDesignedToCartCommand command, CancellationToken cancellationToken)
    {
        var Design = await _designedRepository.Get()
            .Include(c => c.DesignedProduct)
            .ThenInclude(c => c.SizeDetails)
            .FirstOrDefaultAsync(c => c.Id == command.Request.DesignId && !c.IsDeleted, cancellationToken);

        if (Design == null)
            return Result.Failure(new Error("Design.NotFound", "The specified Design does not exist.", 404));

        if (Design.CustomerId != command.CustomerId)
            return Result.Failure(AuthErrors.Forbidden);

        var requestedSizes = command.Request.Sizes.Select(s => s.Size).ToList();
        var availableSizes = Design.DesignedProduct.SizeDetails.Select(s => s.Size).ToList();

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

            foreach (var item in validInitialSizes)
            {
                cartItem.AddOrUpdateSize(item.Size, item.Quantity);
            }

            await _cartItemRepository.CreateAsync(cartItem);
        }
        else
        {
            foreach (var item in command.Request.Sizes)
            {
                cartItem.AddOrUpdateSize(item.Size, item.Quantity);
            }

            if (!cartItem.Sizes.Any())
            {
                await _cartItemRepository.HardDeleteAsync(cartItem);
            }
            else
            {
                await _cartItemRepository.UpdateAsync(cartItem);
            }
        }

        return Result.Success();
    }
}
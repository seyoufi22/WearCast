using WearCast.Api.Features.CartManagment.AddOrUpdateDesignedToCart.DTOs;
namespace WearCast.Api.Features.CartManagment.AddOrUpdateDesignedColorToCart;

public class AddOrUpdateDesignedToCartHandler(IRepository<CartItem> _cartItemRepository, IRepository<CustomerDesign> _designedRepository)
    : IRequestHandler<AddOrUpdateDesignedToCartCommand, Result>
{
    public async Task<Result> Handle(AddOrUpdateDesignedToCartCommand command, CancellationToken cancellationToken)
    {
        var Design = await _designedRepository.Get().Include(c => c.DesignedProduct).ThenInclude(c=>c.SizeDetails)
            .FirstOrDefaultAsync(c => c.Id == command.Request.DesignId && !c.IsDeleted, cancellationToken);

        if (Design == null)
            return Result.Failure(new Error("Design.NotFound", "The specified Design does not exist.", 404));

        if(Design.CustomerId != command.CustomerId)
            return Result.Failure(AuthErrors.Forbidden);

        if (!Design.DesignedProduct.SizeDetails.Any(s => s.Size == command.Request.Size))
            return Result.Failure(new Error("Design.SizeUnavailable", "This size is not available for this product.", 400));

        var cartItem = await _cartItemRepository.Get()
        .Include(c => c.Sizes)
        .FirstOrDefaultAsync(c =>
            c.CustomerDesignId == command.Request.DesignId&&
            c.CustomerId == command.CustomerId,
            cancellationToken);

        if (cartItem == null)
        {
            if (command.Request.Quantity <= 0)
                return Result.Success();

            cartItem = new CartItem
            {
                CustomerId = command.CustomerId,
                CustomerDesignId = command.Request.DesignId
            };

            cartItem.AddOrUpdateSize(command.Request.Size, command.Request.Quantity);

            await _cartItemRepository.CreateAsync(cartItem);
        }
        else
        {
            cartItem.AddOrUpdateSize(command.Request.Size, command.Request.Quantity);

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
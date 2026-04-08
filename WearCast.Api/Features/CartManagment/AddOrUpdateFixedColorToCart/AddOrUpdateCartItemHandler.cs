using WearCast.Api.Features.CartManagment.AddOrUpdateFixedColorToCart.DTOs;
using WearCast.Api.Persistence;

namespace WearCast.Api.Features.CartManagment.AddOrUpdateFixedColorToCart;

public class AddOrUpdateCartItemHandler(
    IRepository<CartItem> _cartItemRepository,
    IRepository<Entities.FixedProduct.FixedProductColor> _colorRepository,
    ApplicationDbContext _dbContext)
    : IRequestHandler<AddOrUpdateFixedColorToCartCommand, Result>
{
    public async Task<Result> Handle(AddOrUpdateFixedColorToCartCommand command, CancellationToken cancellationToken)
    {
        var color = await _colorRepository.Get()
         .Include(c => c.Sizes)
         .FirstOrDefaultAsync(c => c.Id == command.Request.ColorId && !c.IsDeleted, cancellationToken);

        if (color == null)
            return Result.Failure(new Error("Color.NotFound", "The specified color does not exist.", 404));

        if (!color.Sizes.Any(s => s.Size == command.Request.Size))
            return Result.Failure(new Error("Color.SizeUnavailable", "This size is not available for this product.", 400));

        var cartItem = await _cartItemRepository.Get()
        .Include(c => c.Sizes)
        .FirstOrDefaultAsync(c =>
            c.FixedColorId == command.Request.ColorId &&
            c.CustomerId == command.CustomerId,
            cancellationToken);

        if (cartItem == null)
        {
            if (command.Request.Quantity <= 0)
                return Result.Success();

            cartItem = new CartItem
            {
                CustomerId = command.CustomerId,
                FixedColorId = command.Request.ColorId
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
                // Force EF Core to correctly track the JSON state sequence update without corrupting the generic parent object tracking
                _dbContext.Entry(cartItem).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        return Result.Success();
    }
}
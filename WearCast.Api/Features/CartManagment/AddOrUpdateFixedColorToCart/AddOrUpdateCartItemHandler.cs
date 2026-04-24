using WearCast.Api.Features.CartManagment.AddOrUpdateFixedColorToCart.DTOs;

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

        // 1. استخراج المقاسات المطلوبة والمتاحة الموحدة للبرودكت
        var requestedSizes = command.Request.Sizes.Select(s => s.Size).ToList();
        var availableSizes = color.Sizes.Select(s => s.Size).ToList();

        // 2. التحقق من وجود كل المقاسات 
        var unavailableSizes = requestedSizes.Except(availableSizes).ToList();
        if (unavailableSizes.Any())
        {
            var missingSizesStr = string.Join(", ", unavailableSizes);
            // تعديل الرسالة لتوضيح أن المقاس غير متاح في الـ template الخاص بالبرودكت
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
                
                _dbContext.Entry(cartItem).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        return Result.Success();
    }
}
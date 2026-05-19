using WearCast.Api.Features.CartManagment.GetFixedProductsInCart.DTOs;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.CartManagment.GetFixedProductsInCart;

// The return type has been changed here as well
public class GetCartHandler(IRepository<CartItem> cartItemRepository)
    : IRequestHandler<GetCartRequestDto, FixedCartSummaryDto>
{
    public async Task<FixedCartSummaryDto> Handle(GetCartRequestDto request, CancellationToken cancellationToken)
    {
        // 1. Fetch products and calculate total quantity and subtotal per item directly in the database query
        var itemsList = await cartItemRepository.Get()
            .AsNoTracking() // Added for better performance since this is a read-only query
            .Where(c => c.CustomerId == request.CustomerId && c.FixedColorId != null)
            .Select(c => new GetCartItemResponseDto
            {
                unavailable = (c.FixedColor!.IsDeleted) ||
                              (c.FixedColor.Product.IsDeleted) ||
                              (c.FixedColor.Product.Seller.IsDeleted),
                CartItemId = c.Id,
                ProductId = c.FixedColor!.ProductId,
                ProductColorId = c.FixedColorId!.Value,
                ProductName = c.FixedColor.Product.Name,
                Price = c.FixedColor.Product.Price,
                Image = c.FixedColor.ImageUrl,

                // Calculate the total quantity for this single product
                TotalQuantity = c.Sizes.Sum(s => s.Quantity),

                // Calculate the subtotal price for this single product
                SubTotal = c.Sizes.Sum(s => s.Quantity) * c.FixedColor.Product.Price,

                Sizes = c.Sizes.Select(s => new SizeDto(
                    s.Size,
                    s.Quantity,
                    c.FixedColor.Sizes
                        .Where(cs => cs.Size == s.Size)
                        .Select(cs => cs.Quantity)
                        .FirstOrDefault()
                )).ToList()
            })
            .ToListAsync(cancellationToken);

        // 2. Calculate the grand total price for all fixed products combined
        decimal cartTotalPrice = itemsList.Sum(item => item.SubTotal);

        // 3. Assemble and return the final response
        return new FixedCartSummaryDto
        {
            Items = itemsList,
            TotalFixedProductsPrice = cartTotalPrice
        };
    }
}
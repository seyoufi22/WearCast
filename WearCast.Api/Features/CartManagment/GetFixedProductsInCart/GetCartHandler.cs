using WearCast.Api.Features.CartManagment.GetFixedProductsInCart.DTOs;

namespace WearCast.Api.Features.CartManagment.GetFixedProductsInCart;

public class GetCartHandler(IRepository<CartItem> cartItemRepository)
    : IRequestHandler<GetCartRequestDto, List<GetCartItemResponseDto>>
{
    public async Task<List<GetCartItemResponseDto>> Handle(GetCartRequestDto request, CancellationToken cancellationToken)
    {
        var result = await cartItemRepository.Get()
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

        return result;
    }
}
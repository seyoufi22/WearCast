using WearCast.Api.Features.CartManagment.GetDesignsInCart.DTOs;

namespace WearCast.Api.Features.CartManagment.GetDesignsInCart;


public class GetCartHandler(IRepository<CartItem> cartItemRepository)
    : IRequestHandler<GetCartRequestDto, List<GetCartItemResponseDto>>
{
    public async Task<List<GetCartItemResponseDto>> Handle(GetCartRequestDto request, CancellationToken cancellationToken)
    {
        var result = await cartItemRepository.Get()
            .Where(c => c.CustomerId == request.CustomerId && c.CustomerDesignId != null)
            .Select(c => new GetCartItemResponseDto
            {
                CartItemId = c.Id,
                CustomerDesignedId= request.CustomerId,
                ProductName = c.DesignedCustomer!.DesignedProduct.Name,
                Price = c.DesignedCustomer.DesignedProduct.Price,
                Image = c.DesignedCustomer.DesignedProductColor.Images
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault(),
                Sizes = c.Sizes.Select(s => new SizeDto(
                    s.Size,
                    s.Quantity
                )).ToList()
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}

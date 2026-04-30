using WearCast.Api.Features.CartManagment.GetDesignsInCart.DTOs;

namespace WearCast.Api.Features.CartManagment.GetDesignsInCart;


public class GetCartHandler(IRepository<CartItem> cartItemRepository)
    : IRequestHandler<GetCartRequestDto, List<GetCartItemResponseDto>>
{
    public async Task<List<GetCartItemResponseDto>> Handle(GetCartRequestDto request, CancellationToken cancellationToken)
    {
        var cartItems = await cartItemRepository.Get()
        .Where(c => c.CustomerId == request.CustomerId
                 && c.CustomerDesignId != null)
        .Include(c => c.Sizes)
        .Include(c => c.DesignedCustomer!)
            .ThenInclude(d => d.DesignedProduct)
        .Include(c => c.DesignedCustomer!)
            .ThenInclude(d => d.DesignedProductColor)
        .AsNoTracking()
        .ToListAsync(cancellationToken);

        return cartItems.Select(c => new GetCartItemResponseDto
        {
            CartItemId = c.Id,
            CustomerDesignedId = c.CustomerDesignId,
            ProductName = c.DesignedCustomer?.DesignedProduct?.Name,
            Price = c.DesignedCustomer?.DesignedProduct?.Price ?? 0,
            Image = c.DesignedCustomer?.FrontImageUrl
                             ?? c.DesignedCustomer?.BackImageUrl
                             ?? c.DesignedCustomer?.LeftImageUrl
                             ?? c.DesignedCustomer?.RightImageUrl
                             ?? c.DesignedCustomer?.DesignedProductColor?.MainImageUrl
                                    ,
            Sizes = c.Sizes
                            .Select(s => new SizeDto(s.Size, s.Quantity))
                            .ToList()
        }).ToList();
    }
}
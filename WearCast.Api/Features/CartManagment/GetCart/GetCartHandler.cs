using WearCast.Api.Features.CartManagment.GetCart.DTOs;

namespace WearCast.Api.Features.CartManagment.GetCart;

public class GetCartHandler(ApplicationDbContext context)
    : IRequestHandler<GetCartRequestDto, List<GetCartItemResponseDto>>
{
    public async Task<List<GetCartItemResponseDto>> Handle(GetCartRequestDto request, CancellationToken cancellationToken)
    {
        var result = await context.CartItems
            .Where(c => c.CustomerId == request.CustomerId)
            .Select(c => new GetCartItemResponseDto
            {
                IdProduct = c.Color.ProductId,
                IdProductColor = c.ColorId,
                ProductName = c.Color.Product.Name,
                Price = c.Color.Product.Price,
                Image = c.Color.ImageUrl,
                Sizes = c.Sizes.Select(s => new SizeDto(
                    s.Size,
                    s.Quantity,  
                    c.Color.Sizes
                        .Where(cs => cs.Size == s.Size)
                        .Select(cs => cs.Quantity)
                        .FirstOrDefault() 
                )).ToList()
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}
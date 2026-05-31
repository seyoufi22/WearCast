using WearCast.Api.Features.CartManagment.GetMyCart.DTOs;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace WearCast.Api.Features.CartManagment.GetMyCart;

public class GetCartHandler(
    IRepository<CartItem> cartItemRepository,
    ApplicationDbContext dbContext)
    : IRequestHandler<GetCartRequestDto, CartSummaryResponseDto>
{
    public async Task<CartSummaryResponseDto> Handle(GetCartRequestDto request, CancellationToken cancellationToken)
    {
        // ==========================================
        // 1. Fetch Fixed Products
        // ==========================================
        var fixedItems = await cartItemRepository.Get()
            .AsNoTracking() // Reduces memory footprint as change tracking is not needed
            .Where(c => c.CustomerId == request.CustomerId && c.FixedColorId != null)
            .Select(c => new GetFixedCartItemDto
            {
                unavailable = c.FixedColor!.IsDeleted,
                CartItemId = c.Id,
                ProductId = c.FixedColor!.ProductId,
                ProductColorId = c.FixedColorId!.Value,
                ProductName = c.FixedColor.Product.Name,
                Price = c.FixedColor.Product.Price,
                Image = c.FixedColor.ImageUrl,
                Sizes = c.Sizes.OrderBy(s => s.Size)
                .Select(s => new FixedSizeDto(
                    s.Size,
                    s.Quantity,
                    c.FixedColor.Sizes
                        .Where(cs => cs.Size == s.Size)
                        .Select(cs => cs.Quantity)
                        .FirstOrDefault()
                )).ToList()
            })
            .ToListAsync(cancellationToken);

        // ==========================================
        // 2. Fetch Designed Products
        // ==========================================
        // Optimization: Replaced Includes with Select (Projection) to fetch only required data
        var rawDesignedItems = await cartItemRepository.Get()
            .AsNoTracking()
            .Where(c => c.CustomerId == request.CustomerId && c.CustomerDesignId != null)
            .Select(c => new
            {
                CartItemId = c.Id,
                CustomerDesignedId = c.CustomerDesignId,
                // Fetching only required objects for calculation to minimize database load
                Design = c.DesignedCustomer,
                Product = c.DesignedCustomer!.DesignedProduct,
                Color = c.DesignedCustomer!.DesignedProductColor,
                Sizes = c.Sizes.OrderBy(s => s.Size).Select(s => new DesignedSizeDto(s.Size, s.Quantity)).ToList()
            })
            .ToListAsync(cancellationToken);

        var designedItems = rawDesignedItems.Select(c =>
        {
            var design = c.Design;
            var product = c.Product;
            var color = c.Color;



            decimal templatePrice = product?.Price ?? 0;
            decimal totalPrice = design?.TotalPrice ?? templatePrice;
            decimal assetsPrice = totalPrice - templatePrice;
            int assetCount = design?.AssetCount ?? 0;

            string priceDescription = $"[Template: {templatePrice:0.##} EGP + {assetCount} Assets: {assetsPrice:0.##} EGP = {totalPrice:0.##} EGP]";

            return new GetDesignedCartItemDto
            {
                unavailable = (color?.IsDeleted ?? false),
                CartItemId = c.CartItemId,
                CustomerDesignedId = c.CustomerDesignedId,
                ProductName = product?.Name,
                Price = totalPrice,
                PriceDescription = priceDescription,
                Image = design?.FrontImageUrl
                         ?? design?.BackImageUrl
                         ?? design?.LeftImageUrl
                         ?? design?.RightImageUrl
                         ?? color?.MainImageUrl,
                Sizes = c.Sizes
            };
        }).ToList();

        // ==========================================
        // 3. Fetch Delivery Fee
        // ==========================================
        // Optimization: Fetching only the scalar value instead of the entire ShippingCompany entity
        decimal deliveryFee = await dbContext.ShippingCompanies
            .Select(s => s.DeliveryFee)
            .FirstOrDefaultAsync(cancellationToken);

        // ==========================================
        // 4. Assemble Final Response
        // ==========================================
        return new CartSummaryResponseDto
        {
            FixedItems = fixedItems,
            DesignedItems = designedItems,
            DeliveryFee = deliveryFee
        };
    }
}
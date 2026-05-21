using WearCast.Api.Features.CartManagment.GetDesignsInCart.DTOs;
namespace WearCast.Api.Features.CartManagment.GetDesignsInCart;

public class GetCartHandler(
    IRepository<CartItem> cartItemRepository,
    ApplicationDbContext dbContext) // حقنا الـ DbContext عشان نجيب الـ Delivery Fee
    : IRequestHandler<GetCartRequestDto, CartSummaryResponseDto>
{
    public async Task<CartSummaryResponseDto> Handle(GetCartRequestDto request, CancellationToken cancellationToken)
    {
        var cartItems = await cartItemRepository.Get()
            .Where(c => c.CustomerId == request.CustomerId
                     && c.CustomerDesignId != null)
            .Include(c => c.Sizes)
            .Include(c => c.DesignedCustomer!)
                .ThenInclude(d => d.DesignedProduct)
                    .ThenInclude(p => p.Factory)
            .Include(c => c.DesignedCustomer!)
                .ThenInclude(d => d.DesignedProductColor)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // 1. تجهيز لستة المنتجات مع تفاصيل الاسترنج
        var mappedItems = cartItems.Select(c =>
        {
            var design = c.DesignedCustomer;
            var product = design?.DesignedProduct;
            var color = design?.DesignedProductColor;



            decimal templatePrice = product?.Price ?? 0;
            decimal totalPrice = design?.TotalPrice ?? templatePrice;
            decimal assetsPrice = totalPrice - templatePrice;
            int assetCount = design?.AssetCount ?? 0;

            // الاسترنج التفصيلي
            string priceDescription = $"[Template: {templatePrice:0.##} EGP + {assetCount} Assets: {assetsPrice:0.##} EGP = {totalPrice:0.##} EGP]";

            return new GetCartItemResponseDto
            {
                unavailable = (color?.IsDeleted ?? false) ||
                              (product?.IsDeleted ?? false) ||
                              !(product?.IsActive ?? true) ||
                              (product?.Factory?.IsDeleted ?? false),
                CartItemId = c.Id,
                CustomerDesignedId = c.CustomerDesignId,
                ProductName = product?.Name,
                Price = totalPrice, // السعر النهائي للقطعة الواحدة
                PriceDescription = priceDescription,
                Image = design?.FrontImageUrl
                         ?? design?.BackImageUrl
                         ?? design?.LeftImageUrl
                         ?? design?.RightImageUrl
                         ?? color?.MainImageUrl,
                Sizes = c.Sizes.Select(s => new SizeDto(s.Size, s.Quantity)).ToList()
            };
        }).ToList();

        // 2. حساب الـ SubTotal 
        // بنضرب سعر القطعة في مجموع الكميات المطلوبة من كل المقاسات لنفس التصميم
        decimal subTotal = mappedItems.Sum(item => item.Price * item.Sizes.Sum(s => s.QuantityInCart));

        // 3. جلب مصاريف الشحن (بنفس طريقتك في الـ Checkout)
        var shippingCompany = await dbContext.ShippingCompanies.FirstOrDefaultAsync(cancellationToken);
        decimal deliveryFee = shippingCompany?.DeliveryFee ?? 0;

        return new CartSummaryResponseDto
        {
            Items = mappedItems,
            SubTotal = subTotal,
            DeliveryFee = deliveryFee,
            GrandTotal = subTotal + deliveryFee
        };
    }
}
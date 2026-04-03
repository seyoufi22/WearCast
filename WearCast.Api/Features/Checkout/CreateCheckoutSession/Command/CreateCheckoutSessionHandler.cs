using WearCast.Api.Abstractions;
using WearCast.Api.Common.Repository;
using WearCast.Api.Common.Settings;
using WearCast.Api.Entities.Order;
using WearCast.Api.Features.Checkout.CreateCheckoutSession.DTOs;
using Stripe.Checkout;

namespace WearCast.Api.Features.Checkout.CreateCheckoutSession.Command;

public class CreateCheckoutSessionHandler : IRequestHandler<CreateCheckoutSessionRequestDto, Result<CreateCheckoutSessionResponseDto>>
{
    private readonly IRepository<CartItem> _cartRepo;
    private readonly IRepository<Order> _orderRepo;
    private readonly IOptions<StripeSettings> _stripeSettings;

    public CreateCheckoutSessionHandler(
        IRepository<CartItem> cartRepo,
        IRepository<Order> orderRepo,
        IOptions<StripeSettings> stripeSettings)
    {
        _cartRepo = cartRepo;
        _orderRepo = orderRepo;
        _stripeSettings = stripeSettings;
    }

    public async Task<Result<CreateCheckoutSessionResponseDto>> Handle(CreateCheckoutSessionRequestDto request, CancellationToken cancellationToken)
    {
        // 1. Load all fixed-product cart items for this customer
        var cartItems = await _cartRepo.Get()
            .Where(c => c.CustomerId == request.CustomerId && c.FixedColorId != null)
            .Include(c => c.FixedColor!)
                .ThenInclude(fc => fc.Product)
            .Include(c => c.FixedColor!)
                .ThenInclude(fc => fc.Sizes)
            .Include(c => c.Sizes)
            .ToListAsync(cancellationToken);

        if (!cartItems.Any())
        {
            return Result.Failure<CreateCheckoutSessionResponseDto>(
                new Error("Checkout.EmptyCart", "Your cart has no products to checkout.", StatusCodes.Status400BadRequest));
        }

        // 2. Group cart items by seller
        var itemsBySeller = cartItems.GroupBy(c => c.FixedColor!.Product.SellerId);

        var orders = new List<Order>();
        var stripeLineItems = new List<SessionLineItemOptions>();

        foreach (var sellerGroup in itemsBySeller)
        {
            var sellerId = sellerGroup.Key;
            var orderItems = new List<FixedProductOrderItem>();

            foreach (var cartItem in sellerGroup)
            {
                var product = cartItem.FixedColor!.Product;
                var color = cartItem.FixedColor!;

                // Flatten each size into a separate order item
                foreach (var size in cartItem.Sizes)
                {
                    var orderItem = new FixedProductOrderItem
                    {
                        FixedColorId = color.Id,
                        ProductName = product.Name,
                        ColorName = color.ColorName,
                        ImageUrl = color.ImageUrl,
                        SizeName = size.Size.ToString(),
                        Quantity = size.Quantity,
                        UnitPrice = product.Price
                    };
                    orderItems.Add(orderItem);

                    // Add corresponding Stripe line item
                    stripeLineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "egp",
                            UnitAmountDecimal = product.Price * 100, // Stripe expects amount in piasters
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"{product.Name} - {color.ColorName} ({size.Size})",
                                Images = !string.IsNullOrEmpty(color.ImageUrl)
                                    ? new List<string> { color.ImageUrl }
                                    : null
                            }
                        },
                        Quantity = size.Quantity
                    });
                }
            }

            // 3. Create the order for this seller
            var order = new Order
            {
                CustomerId = request.CustomerId,
                SellerId = sellerId,
                TotalAmount = orderItems.Sum(i => i.UnitPrice * i.Quantity),
                Status = OrderStatus.Pending,
                FixedProductItems = orderItems,
                RecipientName = request.ShippingInfo.RecipientName,
                RecipientPhoneNumber = request.ShippingInfo.PhoneNumber,
                RecipientAdditionalPhoneNumber = request.ShippingInfo.AdditionalPhoneNumber,
                ShippingAddress = new Address
                {
                    State = request.ShippingInfo.State,
                    City = request.ShippingInfo.City,
                    Street = request.ShippingInfo.Street,
                    BuildingNumber = request.ShippingInfo.BuildingNumber
                }
            };
            orders.Add(order);
        }

        // 4. Create Stripe Checkout Session
        var sessionOptions = new SessionCreateOptions
        {
            CustomerEmail = request.CustomerEmail,
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = stripeLineItems,
            Mode = "payment",
            SuccessUrl = _stripeSettings.Value.SuccessUrl,
            CancelUrl = _stripeSettings.Value.CancelUrl
        };

        var service = new SessionService();
        var session = await service.CreateAsync(sessionOptions, cancellationToken: cancellationToken);

        // 5. Save StripeSessionId on all orders and persist
        foreach (var order in orders)
        {
            order.StripeSessionId = session.Id;
            await _orderRepo.CreateAsync(order);
        }

        // 6. Clear the customer's fixed-product cart items
        foreach (var cartItem in cartItems)
        {
            await _cartRepo.HardDeleteAsync(cartItem);
        }

        return Result.Success(new CreateCheckoutSessionResponseDto
        {
            CheckoutUrl = session.Url,
            OrderIds = orders.Select(o => o.Id).ToList()
        });
    }
}

using Stripe;
using WearCast.Api.Abstractions;
using WearCast.Api.Common.Repository;
using WearCast.Api.Common.Settings;
using WearCast.Api.Entities.Order;
using WearCast.Api.Features.Checkout.CreateCheckoutSession.DTOs;
using WearCast.Api.Persistence;
using Stripe.Checkout;

namespace WearCast.Api.Features.Checkout.CreateCheckoutSession.Command;

public class CreateCheckoutSessionHandler : IRequestHandler<CreateCheckoutSessionRequestDto, Result<CreateCheckoutSessionResponseDto>>
{
    private readonly IRepository<CartItem> _cartRepo;
    private readonly IRepository<Order> _orderRepo;
    private readonly IOptions<StripeSettings> _stripeSettings;
    private readonly ApplicationDbContext _dbContext;

    public CreateCheckoutSessionHandler(
        IRepository<CartItem> cartRepo,
        IRepository<Order> orderRepo,
        IOptions<StripeSettings> stripeSettings,
        ApplicationDbContext dbContext)
    {
        _cartRepo = cartRepo;
        _orderRepo = orderRepo;
        _stripeSettings = stripeSettings;
        _dbContext = dbContext;
    }

    public async Task<Result<CreateCheckoutSessionResponseDto>> Handle(CreateCheckoutSessionRequestDto request, CancellationToken cancellationToken)
    {
        // 1. Load all cart items for this customer
        var fixedCartItems = await _cartRepo.Get()
            .Where(c => c.CustomerId == request.CustomerId && c.FixedColorId != null)
            .Include(c => c.FixedColor!)
                .ThenInclude(fc => fc.Product)
            .Include(c => c.FixedColor!)
                .ThenInclude(fc => fc.Sizes)
            .Include(c => c.Sizes)
            .ToListAsync(cancellationToken);

        var designedCartItems = await _cartRepo.Get()
            .Where(c => c.CustomerId == request.CustomerId && c.CustomerDesignId != null)
            .Include(c => c.DesignedCustomer!)
                .ThenInclude(cd => cd.DesignedProduct)
            .Include(c => c.DesignedCustomer!)
                .ThenInclude(cd => cd.DesignedProductColor)
            .Include(c => c.Sizes)
            .ToListAsync(cancellationToken);

        if (!fixedCartItems.Any() && !designedCartItems.Any())
        {
            return Result.Failure<CreateCheckoutSessionResponseDto>(
                new Error("Checkout.EmptyCart", "Your cart has no products to checkout.", StatusCodes.Status400BadRequest));
        }

        var existingPendingOrders = await _dbContext.Orders
            .Include(o => o.FixedProductItems)
            .Include(o => o.DesignedProductItems)
            .Where(o => o.CustomerId == request.CustomerId && o.Status == OrderStatus.Pending)
            .ToListAsync(cancellationToken);

        if (existingPendingOrders.Any())
        {
            var existingFixedItems = existingPendingOrders.SelectMany(o => o.FixedProductItems).ToList();
            if (existingFixedItems.Any())
                _dbContext.FixedProductOrderItems.RemoveRange(existingFixedItems);

            var existingDesignedItems = existingPendingOrders.SelectMany(o => o.DesignedProductItems).ToList();
            if (existingDesignedItems.Any())
                _dbContext.CustomerDesignedOrderItems.RemoveRange(existingDesignedItems);

            _dbContext.Orders.RemoveRange(existingPendingOrders);
        }

        var orders = new List<Order>();
        var stripeLineItems = new List<SessionLineItemOptions>();

        // Load a shipping company to retrieve the delivery fee
        var shippingCompany = await _dbContext.ShippingCompanies.FirstOrDefaultAsync(cancellationToken);

        // Add delivery fee as a dedicated Stripe line item (once per session)
        if (shippingCompany != null && shippingCompany.DeliveryFee > 0)
        {
            stripeLineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "egp",
                    UnitAmountDecimal = shippingCompany.DeliveryFee * 100,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Delivery Fee"
                    }
                },
                Quantity = 1
            });
        }

        // 2. Process fixed product cart items (group by seller)
        if (fixedCartItems.Any())
        {
            var itemsBySeller = fixedCartItems.GroupBy(c => c.FixedColor!.Product.SellerId);

            var sellerIds = itemsBySeller.Select(g => g.Key).ToList();
            var sellers = await _dbContext.Sellers
                .Where(s => sellerIds.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id, cancellationToken);

            foreach (var sellerGroup in itemsBySeller)
            {
                var sellerId = sellerGroup.Key;
                var seller = sellers[sellerId];
                var orderItems = new List<FixedProductOrderItem>();

                foreach (var cartItem in sellerGroup)
                {
                    var product = cartItem.FixedColor!.Product;
                    var color = cartItem.FixedColor!;

                    foreach (var size in cartItem.Sizes)
                    {
                        var stockSize = color.Sizes.FirstOrDefault(s => s.Size == size.Size);
                        if (stockSize == null || stockSize.Quantity < size.Quantity)
                        {
                            return Result.Failure<CreateCheckoutSessionResponseDto>(
                                new Error("Checkout.InsufficientStock",
                                    $"Insufficient stock for {product.Name} - {color.ColorName} ({size.Size}). Available: {stockSize?.Quantity ?? 0}, Requested: {size.Quantity}",
                                    StatusCodes.Status400BadRequest));
                        }

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

                        stripeLineItems.Add(new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "egp",
                                UnitAmountDecimal = product.Price * 100,
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
                    ShippingAddress = new WearCast.Api.Common.ValueObjects.Address
                    {
                        State = request.ShippingInfo.State,
                        City = request.ShippingInfo.City,
                        Street = request.ShippingInfo.Street,
                        BuildingNumber = request.ShippingInfo.BuildingNumber
                    },
                    PickUpAddress = new WearCast.Api.Common.ValueObjects.Address
                    {
                        State = seller.Address.State,
                        City = seller.Address.City,
                        Street = seller.Address.Street,
                        BuildingNumber = seller.Address.BuildingNumber
                    }
                };
                orders.Add(order);
            }
        }

        if (designedCartItems.Any())
        {
            var factory = await _dbContext.Factories.FirstOrDefaultAsync(cancellationToken);
            if (factory == null)
            {
                return Result.Failure<CreateCheckoutSessionResponseDto>(
                    new Error("Checkout.NoFactory", "No factory found to process designed product orders.", StatusCodes.Status400BadRequest));
            }

            var designedOrderItems = new List<CustomerDesignedOrderItem>();

            foreach (var cartItem in designedCartItems)
            {
                var design = cartItem.DesignedCustomer!;
                var product = design.DesignedProduct;
                var color = design.DesignedProductColor;

                design.CalculateAndSetTotalPrice(product.Price, design.AssetCount);
                
                foreach (var size in cartItem.Sizes)
                {
                    var orderItem = new CustomerDesignedOrderItem
                    {
                        CustomerDesignId = design.Id,
                        DesignedProductId = design.DesignedProductId,
                        ProductName = product.Name,
                        ColorName = color.Name,
                        SizeName = size.Size.ToString(),
                        Quantity = size.Quantity,
                        UnitPrice = design.TotalPrice,
                        FrontImageUrl = design.FrontImageUrl,
                        BackImageUrl = design.BackImageUrl,
                        RightImageUrl = design.RightImageUrl,
                        LeftImageUrl = design.LeftImageUrl,
                        ViewDesignsJson = design.ViewDesignsJson
                    };
                    designedOrderItems.Add(orderItem);

                    stripeLineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "egp",
                            UnitAmountDecimal = design.TotalPrice * 100,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"{product.Name} - {color.Name} ({size.Size}) [Template: {product.Price:C} + {design.AssetCount} Assets: {design.TotalPrice - product.Price:C} = {design.TotalPrice:C}]"
                            }
                        },
                        Quantity = size.Quantity
                    });
                }
            }

            var designedOrder = new Order
            {
                CustomerId = request.CustomerId,
                FactoryId = factory.Id,
                TotalAmount = designedOrderItems.Sum(i => i.UnitPrice * i.Quantity),
                Status = OrderStatus.Pending,
                DesignedProductItems = designedOrderItems,
                RecipientName = request.ShippingInfo.RecipientName,
                RecipientPhoneNumber = request.ShippingInfo.PhoneNumber,
                RecipientAdditionalPhoneNumber = request.ShippingInfo.AdditionalPhoneNumber,
                ShippingAddress = new WearCast.Api.Common.ValueObjects.Address
                {
                    State = request.ShippingInfo.State,
                    City = request.ShippingInfo.City,
                    Street = request.ShippingInfo.Street,
                    BuildingNumber = request.ShippingInfo.BuildingNumber
                },
                PickUpAddress = new WearCast.Api.Common.ValueObjects.Address
                {
                    State = factory.Address.State,
                    City = factory.Address.City,
                    Street = factory.Address.Street,
                    BuildingNumber = factory.Address.BuildingNumber
                }
            };
            orders.Add(designedOrder);
        }



        var customerService = new CustomerService();
        var stripeCustomers = await customerService.ListAsync(
            new CustomerListOptions { Email = request.CustomerEmail }, 
            cancellationToken: cancellationToken);
        
        var existingCustomer = stripeCustomers.FirstOrDefault();

        var sessionOptions = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = stripeLineItems,
            Mode = "payment",
            SuccessUrl = _stripeSettings.Value.SuccessUrl,
            CancelUrl = _stripeSettings.Value.CancelUrl
        };

        if (existingCustomer != null)
        {
            sessionOptions.Customer = existingCustomer.Id;
        }
        else
        {
            sessionOptions.CustomerEmail = request.CustomerEmail;
            sessionOptions.CustomerCreation = "always";
        }

        var service = new SessionService();
        var session = await service.CreateAsync(sessionOptions, cancellationToken: cancellationToken);

        foreach (var order in orders)
        {
            order.StripeSessionId = session.Id;
        }

        _dbContext.Orders.AddRange(orders);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreateCheckoutSessionResponseDto
        {
            CheckoutUrl = session.Url,
            OrderIds = orders.Select(o => o.Id).ToList()
        });
    }
}
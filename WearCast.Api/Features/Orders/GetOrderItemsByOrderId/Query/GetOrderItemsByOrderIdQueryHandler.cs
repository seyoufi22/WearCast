using WearCast.Api.Abstractions;
using WearCast.Api.Features.Orders.GetOrderItemsByOrderId.DTOs;
using WearCast.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Orders.GetOrderItemsByOrderId.Query;

public class GetOrderItemsByOrderIdQueryHandler(ApplicationDbContext dbContext) : IRequestHandler<GetOrderItemsByOrderIdQuery, Result<GetOrderItemsByOrderIdResponseDto>>
{
    public async Task<Result<GetOrderItemsByOrderIdResponseDto>> Handle(GetOrderItemsByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(o => o.FixedProductItems)
            .Include(o => o.DesignedProductItems)
            .Where(o => o.Id == request.OrderId && !o.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null)
            return Result.Failure<GetOrderItemsByOrderIdResponseDto>(new Error("Orders.NotFound", $"Order with ID {request.OrderId} not found.", Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound));
            
        var detailsDto = new GetOrderItemsByOrderIdResponseDto
        {
            Id = order.Id,
            TotalAmount = order.TotalAmount,
            Commission = order.Commission,
            Payout = order.Payout,
            Status = order.Status,
            CreatedOn = order.CreatedOn,
            RecipientName = order.RecipientName,
            RecipientPhoneNumber = order.RecipientPhoneNumber,
            RecipientAdditionalPhoneNumber = order.RecipientAdditionalPhoneNumber,
            ShippingAddress = order.ShippingAddress,
            TotalOrderItems = order.FixedProductItems.Count + order.DesignedProductItems.Count,
            Items = order.FixedProductItems
                .GroupBy(i => i.FixedColorId)
                .Select(g => new OrderItemDto
                {
                    FixedColorId = g.Key,
                    ProductName = g.First().ProductName,
                    ColorName = g.First().ColorName,
                    UnitPrice = g.First().UnitPrice,
                    ImageUrl = g.First().ImageUrl,
                    TotalQuantity = g.Sum(i => i.Quantity),
                    TotalPrice = g.Sum(i => i.UnitPrice * i.Quantity),
                    Sizes = g.Select(i => new OrderItemSizeDto
                    {
                        Id = i.Id,
                        SizeName = i.SizeName,
                        Quantity = i.Quantity
                    }).ToList()
                }).ToList(),
            DesignedItems = order.DesignedProductItems
                .GroupBy(d => d.CustomerDesignId)
                .Select(g => new DesignedOrderItemDto
                {
                    CustomerDesignId = g.Key,
                    ProductName = g.First().ProductName,
                    ColorName = g.First().ColorName,
                    UnitPrice = g.First().UnitPrice,
                    TotalQuantity = g.Sum(d => d.Quantity),
                    TotalPrice = g.Sum(d => d.UnitPrice * d.Quantity),
                    FrontImageUrl = g.First().FrontImageUrl,
                    BackImageUrl = g.First().BackImageUrl,
                    RightImageUrl = g.First().RightImageUrl,
                    LeftImageUrl = g.First().LeftImageUrl,
                    ViewDesignsJson = g.First().ViewDesignsJson,
                    Sizes = g.Select(d => new OrderItemSizeDto
                    {
                        Id = d.Id,
                        SizeName = d.SizeName,
                        Quantity = d.Quantity
                    }).ToList()
                }).ToList()
        };

        return Result<GetOrderItemsByOrderIdResponseDto>.Success(detailsDto);
    }
}

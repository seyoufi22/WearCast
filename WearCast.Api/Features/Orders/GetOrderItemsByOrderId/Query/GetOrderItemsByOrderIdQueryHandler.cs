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

        // Security check: only the buyer, the seller, or the factory of this order can view the items.
        bool hasAccess = false;
        if (request.CustomerId.HasValue && order.CustomerId == request.CustomerId.Value)
            hasAccess = true;
        else if (request.SellerId.HasValue && order.SellerId.HasValue && order.SellerId == request.SellerId.Value)
            hasAccess = true;
        else if (request.FactoryId.HasValue && order.FactoryId.HasValue && order.FactoryId == request.FactoryId.Value)
            hasAccess = true;

        if (!hasAccess)
        {
            return Result.Failure<GetOrderItemsByOrderIdResponseDto>(new Error("Orders.Forbidden", "You do not have permission to view this order.", Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden));
        }

        var detailsDto = new GetOrderItemsByOrderIdResponseDto
        {
            Id = order.Id,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            CreatedOn = order.CreatedOn,
            RecipientName = order.RecipientName,
            RecipientPhoneNumber = order.RecipientPhoneNumber,
            RecipientAdditionalPhoneNumber = order.RecipientAdditionalPhoneNumber,
            ShippingAddress = order.ShippingAddress,
            Items = order.FixedProductItems.Select(i => new OrderItemDto
            {
                Id = i.Id,
                FixedColorId = i.FixedColorId,
                ProductName = i.ProductName,
                ColorName = i.ColorName,
                SizeName = i.SizeName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                ImageUrl = i.ImageUrl
            }).ToList(),
            DesignedItems = order.DesignedProductItems.Select(d => new DesignedOrderItemDto
            {
                Id = d.Id,
                CustomerDesignId = d.CustomerDesignId,
                ProductName = d.ProductName,
                ColorName = d.ColorName,
                SizeName = d.SizeName,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                FrontImageUrl = d.FrontImageUrl,
                BackImageUrl = d.BackImageUrl,
                RightImageUrl = d.RightImageUrl,
                LeftImageUrl = d.LeftImageUrl,
                ViewDesignsJson = d.ViewDesignsJson
            }).ToList()
        };

        return Result<GetOrderItemsByOrderIdResponseDto>.Success(detailsDto);
    }
}

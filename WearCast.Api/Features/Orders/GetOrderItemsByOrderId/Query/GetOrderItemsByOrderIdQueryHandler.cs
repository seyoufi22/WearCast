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
            .Where(o => o.Id == request.OrderId && !o.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null)
            return Result.Failure<GetOrderItemsByOrderIdResponseDto>(new Error("Orders.NotFound", $"Order with ID {request.OrderId} not found.", Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound));

        // Security check: only the buyer or the seller of this order can view the items.
        if ((request.CustomerId.HasValue && order.CustomerId != request.CustomerId.Value) || 
            (request.SellerId.HasValue && order.SellerId != request.SellerId.Value) ||
            (!request.CustomerId.HasValue && !request.SellerId.HasValue))
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
            }).ToList()
        };

        return Result<GetOrderItemsByOrderIdResponseDto>.Success(detailsDto);
    }
}

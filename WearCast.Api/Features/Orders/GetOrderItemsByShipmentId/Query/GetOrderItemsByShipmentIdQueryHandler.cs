using WearCast.Api.Abstractions;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Orders.GetOrderItemsByShipmentId.DTOs;
using WearCast.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Orders.GetOrderItemsByShipmentId.Query;

public class GetOrderItemsByShipmentIdQueryHandler(ApplicationDbContext dbContext)
    : IRequestHandler<GetOrderItemsByShipmentIdQuery, Result<GetOrderItemsByShipmentIdResponseDto>>
{
    public async Task<Result<GetOrderItemsByShipmentIdResponseDto>> Handle(
        GetOrderItemsByShipmentIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Verify shipment exists
        var shipment = await dbContext.Shipments
            .AsNoTracking()
            .Where(s => s.Id == request.ShipmentId && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (shipment == null)
            return Result.Failure<GetOrderItemsByShipmentIdResponseDto>(
                new Error("Shipments.NotFound", $"Shipment with ID {request.ShipmentId} not found.",
                    Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound));

        // 2. Security check
        if (request.CustomerId.HasValue && shipment.CustomerId != request.CustomerId.Value)
            return Result.Failure<GetOrderItemsByShipmentIdResponseDto>(
                new Error("Shipments.Forbidden", "You do not have permission to view this shipment.",
                    Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden));

        // 3. Load all orders for this shipment
        var orders = await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.FixedProductItems)
            .Include(o => o.DesignedProductItems)
            .Where(o => o.ShipmentId == request.ShipmentId && !o.IsDeleted)
            .ToListAsync(cancellationToken);

        // For seller/factory access, verify they own at least one order in this shipment
        if (request.SellerId.HasValue && !orders.Any(o => o.SellerId == request.SellerId.Value))
            return Result.Failure<GetOrderItemsByShipmentIdResponseDto>(
                new Error("Shipments.Forbidden", "You do not have permission to view this shipment.",
                    Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden));

        if (request.FactoryId.HasValue && !orders.Any(o => o.FactoryId == request.FactoryId.Value))
            return Result.Failure<GetOrderItemsByShipmentIdResponseDto>(
                new Error("Shipments.Forbidden", "You do not have permission to view this shipment.",
                    Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden));

        // 4. Flatten and group fixed order items by FixedColorId
        var allFixedItems = orders
            .SelectMany(o => o.FixedProductItems)
            .ToList();

        var fixedItems = allFixedItems
            .GroupBy(i => i.FixedColorId)
            .Select(g => new FixedProductOrderItemDto
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
                    OrderId = i.OrderId,
                    SizeName = i.SizeName,
                    Quantity = i.Quantity
                }).ToList()
            }).AsQueryable();

        // 5. Flatten and group designed order items by CustomerDesignId
        var allDesignedItems = orders
            .SelectMany(o => o.DesignedProductItems)
            .ToList();

        var designedItems = allDesignedItems
            .GroupBy(d => d.CustomerDesignId)
            .Select(g => new DesignedProductOrderItemDto
            {
                CustomerDesignId = g.Key,
                DesignedProductId = g.First().DesignedProductId,
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
                    OrderId = d.OrderId,
                    SizeName = d.SizeName,
                    Quantity = d.Quantity
                }).ToList()
            }).AsQueryable();

        // 6. Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            fixedItems = fixedItems.Where(i => i.ProductName.ToLower().Contains(term));
            designedItems = designedItems.Where(i => i.ProductName.ToLower().Contains(term));
        }

        // 7. Apply sorting to fixed items
        fixedItems = request.SortBy?.ToLower() switch
        {
            "productname" => request.SortDescending
                ? fixedItems.OrderByDescending(i => i.ProductName)
                : fixedItems.OrderBy(i => i.ProductName),
            "unitprice" => request.SortDescending
                ? fixedItems.OrderByDescending(i => i.UnitPrice)
                : fixedItems.OrderBy(i => i.UnitPrice),
            "quantity" => request.SortDescending
                ? fixedItems.OrderByDescending(i => i.TotalQuantity)
                : fixedItems.OrderBy(i => i.TotalQuantity),
            _ => request.SortDescending
                ? fixedItems.OrderByDescending(i => i.FixedColorId)
                : fixedItems.OrderBy(i => i.FixedColorId)
        };

        // 8. Apply sorting to designed items
        designedItems = request.SortBy?.ToLower() switch
        {
            "productname" => request.SortDescending
                ? designedItems.OrderByDescending(i => i.ProductName)
                : designedItems.OrderBy(i => i.ProductName),
            "unitprice" => request.SortDescending
                ? designedItems.OrderByDescending(i => i.UnitPrice)
                : designedItems.OrderBy(i => i.UnitPrice),
            "quantity" => request.SortDescending
                ? designedItems.OrderByDescending(i => i.TotalQuantity)
                : designedItems.OrderBy(i => i.TotalQuantity),
            _ => request.SortDescending
                ? designedItems.OrderByDescending(i => i.CustomerDesignId)
                : designedItems.OrderBy(i => i.CustomerDesignId)
        };

        // 9. Paginate fixed items
        var fixedTotal = fixedItems.Count();
        var fixedPaged = fixedItems
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // 10. Paginate designed items
        var designedTotal = designedItems.Count();
        var designedPaged = designedItems
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var response = new GetOrderItemsByShipmentIdResponseDto
        {
            FixedItems = new PagingViewModel<FixedProductOrderItemDto>
            {
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                Records = fixedTotal,
                Pages = (int)Math.Ceiling(fixedTotal / (double)request.PageSize),
                Items = fixedPaged
            },
            DesignedItems = new PagingViewModel<DesignedProductOrderItemDto>
            {
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                Records = designedTotal,
                Pages = (int)Math.Ceiling(designedTotal / (double)request.PageSize),
                Items = designedPaged
            }
        };

        return Result<GetOrderItemsByShipmentIdResponseDto>.Success(response);
    }
}

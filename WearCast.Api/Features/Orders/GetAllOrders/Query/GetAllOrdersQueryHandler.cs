using WearCast.Api.Abstractions;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Orders.GetAllOrders.DTOs;
using WearCast.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Orders.GetAllOrders.Query;

public class GetAllOrdersQueryHandler(ApplicationDbContext dbContext)
    : IRequestHandler<GetAllOrdersQuery, Result<PagingViewModel<GetAllOrdersResponseDto>>>
{
    public async Task<Result<PagingViewModel<GetAllOrdersResponseDto>>> Handle(
        GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Orders
            .AsNoTracking()
            .Where(o => !o.IsDeleted && o.Status != OrderStatus.Pending)
            .AsQueryable();

        // Filter by owner
        if (request.SellerId.HasValue)
            query = query.Where(o => o.SellerId == request.SellerId.Value);
        else if (request.FactoryId.HasValue)
            query = query.Where(o => o.FactoryId == request.FactoryId.Value);

        // Filter by status
        if (request.StatusFilter.HasValue)
            query = query.Where(o => o.Status == request.StatusFilter.Value);

        // Sort
        query = request.SortBy?.ToLower() switch
        {
            "totalamount" => request.SortDescending
                ? query.OrderByDescending(o => o.TotalAmount)
                : query.OrderBy(o => o.TotalAmount),
            "status" => request.SortDescending
                ? query.OrderByDescending(o => o.Status)
                : query.OrderBy(o => o.Status),
            _ => request.SortDescending
                ? query.OrderByDescending(o => o.CreatedOn)
                : query.OrderBy(o => o.CreatedOn)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(o => new GetAllOrdersResponseDto
            {
                Id = o.Id,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedOn = o.CreatedOn,
                RecipientName = o.RecipientName,
                RecipientPhoneNumber = o.RecipientPhoneNumber,
                ShippingAddress = o.ShippingAddress,
                TotalOrderItems = o.FixedProductItems.Count + o.DesignedProductItems.Count,
                OrderType = o.SellerId.HasValue ? "Fixed" : "Designed"
            })
            .ToListAsync(cancellationToken);

        var pagingViewModel = new PagingViewModel<GetAllOrdersResponseDto>
        {
            PageIndex = request.PageNumber,
            PageSize = request.PageSize,
            Records = totalCount,
            Pages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            Items = orders
        };

        return Result<PagingViewModel<GetAllOrdersResponseDto>>.Success(pagingViewModel);
    }
}

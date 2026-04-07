using WearCast.Api.Abstractions;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Orders.GetOrdersByCustomerId.DTOs;
using WearCast.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Orders.GetOrdersByCustomerId.Query;

public class GetOrdersByCustomerIdQueryHandler(ApplicationDbContext dbContext) : IRequestHandler<GetOrdersByCustomerIdQuery, Result<PagingViewModel<GetOrdersByCustomerIdResponseDto>>>
{
    public async Task<Result<PagingViewModel<GetOrdersByCustomerIdResponseDto>>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == request.CustomerId && !o.IsDeleted && o.Status != OrderStatus.Pending)
            .AsQueryable();

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
            .Select(o => new GetOrdersByCustomerIdResponseDto
            {
                Id = o.Id,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedOn = o.CreatedOn,
                RecipientName = o.RecipientName,
                RecipientPhoneNumber = o.RecipientPhoneNumber,
                ShippingAddress = o.ShippingAddress
            })
            .ToListAsync(cancellationToken);

        var pagingViewModel = new PagingViewModel<GetOrdersByCustomerIdResponseDto>
        {
            PageIndex = request.PageNumber,
            PageSize = request.PageSize,
            Records = totalCount,
            Pages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            Items = orders
        };

        return Result<PagingViewModel<GetOrdersByCustomerIdResponseDto>>.Success(pagingViewModel);
    }
}

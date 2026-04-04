using WearCast.Api.Abstractions;
using WearCast.Api.Common.Enums;
using WearCast.Api.Features.Checkout.StripeWebhook.DTOs;
using WearCast.Api.Persistence;

namespace WearCast.Api.Features.Checkout.StripeWebhook.Command;

public class StripeWebhookHandler(ApplicationDbContext dbContext) : IRequestHandler<StripeWebhookRequestDto, Result<bool>>
{
    public async Task<Result<bool>> Handle(StripeWebhookRequestDto request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(o => o.FixedProductItems)
            .Where(o => o.StripeSessionId == request.StripeSessionId)
            .ToListAsync(cancellationToken);

        if (!orders.Any())
            return Result.Failure<bool>(new Error("Orders.NotFound", $"No orders found for session {request.StripeSessionId}", Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound));

        foreach (var order in orders)
        {
            if (order.Status == OrderStatus.Paid) continue;
            
            order.Status = OrderStatus.Paid;

            // Decrement quantities now that payment is successful
            foreach (var item in order.FixedProductItems)
            {
                var color = await dbContext.FixedProductColors
                    .Include(c => c.Sizes)
                    .FirstOrDefaultAsync(c => c.Id == item.FixedColorId, cancellationToken);
                
                if (color != null)
                {
                    var sizeDetail = color.Sizes.FirstOrDefault(s => s.Size.ToString() == item.SizeName);
                    if (sizeDetail != null && sizeDetail.Quantity >= item.Quantity)
                    {
                        sizeDetail.Quantity -= item.Quantity;
                    }

                    // Force EF to re-serialize the JSON-owned Sizes collection
                    dbContext.Entry(color).State = EntityState.Modified;
                }
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}






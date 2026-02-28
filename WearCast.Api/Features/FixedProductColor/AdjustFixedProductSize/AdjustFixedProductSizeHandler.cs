using WearCast.Api.Features.FixedProductColor.AdjustFixedProductSize.DTOs;
public class AdjustFixedProductSizeStockHandler : IRequestHandler<AdjustFixedProductSizeRequestDto, bool>
{
    private readonly ApplicationDbContext _context;

    public AdjustFixedProductSizeStockHandler(ApplicationDbContext context) => _context = context;

    public async Task<bool> Handle(AdjustFixedProductSizeRequestDto request, CancellationToken cancellationToken)
    {
        var rowsAffected = await _context.FixedProductSizes
            .Where(s => s.ProductColorId == request.ProductColorId
                     && s.Size == request.Size
                     && (request.Amount >= 0 || s.Quantity >= Math.Abs(request.Amount)))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.Quantity, s => s.Quantity + request.Amount),
                cancellationToken);

        return rowsAffected > 0;
    }
}
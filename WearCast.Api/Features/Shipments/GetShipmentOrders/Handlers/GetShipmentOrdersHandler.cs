using WearCast.Api.Features.Drivers;
using WearCast.Api.Features.Shipments.GetShipmentOrders.DTOs;

namespace WearCast.Api.Features.Shipments.GetShipmentOrders.Handlers
{
    public class GetShipmentOrdersHandler : IRequestHandler<GetShipmentOrdersRequestDTO, Result<List<GetShipmentOrdersResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;
        public GetShipmentOrdersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<GetShipmentOrdersResponseDTO>>> Handle(GetShipmentOrdersRequestDTO request, CancellationToken cancellationToken)
        {
            var shipmentInfo = await _context.Shipments
                .AsNoTracking()
                .Where(s => s.Id == request.ShipmentId)
                .Select(s => new { s.Id, s.DriverId })
                .FirstOrDefaultAsync(cancellationToken);

            if (shipmentInfo == null)
            {
                return Result.Failure<List<GetShipmentOrdersResponseDTO>>(ShipmentErrors.NotFound);
            }

            if (request.DriverId.HasValue)
            {
                if (shipmentInfo.DriverId != request.DriverId.Value)
                {
                    return Result.Failure<List<GetShipmentOrdersResponseDTO>>(DriverErrors.UnAuthorized);
                }
            }

            var query = _context.Orders
                 .Where(o => o.ShipmentId == request.ShipmentId)
                 .AsNoTracking();

            if (request.OrderStatus.HasValue)
            {
                query = query.Where(o => o.Status == request.OrderStatus.Value);
            }
            if (request.OrderType.HasValue)
            {
                query = request.OrderType.Value == OrderType.Fixed
                    ? query.Where(o => o.SellerId.HasValue)
                    : query.Where(o => !o.SellerId.HasValue);
            }
            var ordersList = await query
                .Select(o => new GetShipmentOrdersResponseDTO
                {
                    OrderId = o.Id,
                    OrderType = o.SellerId.HasValue ? OrderType.Fixed : OrderType.Designed,

                    VendorName = o.Seller != null ? o.Seller.Name : (o.Factory != null ? o.Factory.Name : string.Empty),

                    VendorPhoneNumber = o.Seller != null ? o.Seller.PhoneNumber : (o.Factory != null ? o.Factory.PhoneNumber : string.Empty),

                    VendorAddress = o.Seller != null ? o.Seller.Address : (o.Factory != null ? o.Factory.Address : new Address()),

                    OrderStatus = o.Status,
                    NumberOfItems = o.FixedProductItems.Count+o.DesignedProductItems.Count 
                })
                .ToListAsync(cancellationToken);

            return Result.Success(ordersList);
        }
    }
}

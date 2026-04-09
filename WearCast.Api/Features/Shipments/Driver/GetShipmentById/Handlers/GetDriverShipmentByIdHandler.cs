using WearCast.Api.Features.Drivers;
using WearCast.Api.Features.Drivers.GetDriverById.DTOs;
using WearCast.Api.Features.Shipments.Driver.GetShipmentById.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.GetShipmentById.Handlers
{
    public class GetDriverShipmentByIdHandler : IRequestHandler<GetDriverShipmentByIdRequestDTO, Result<GetDriverShipmentByIdResponseDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetDriverShipmentByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<GetDriverShipmentByIdResponseDTO>> Handle(
            GetDriverShipmentByIdRequestDTO request,
            CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .AsNoTracking()
                .Where(s => 
                s.Id == request.ShipmentId && s.DriverId==request.DriverId)
                .Select(s => new GetDriverShipmentByIdResponseDTO
                {
                    Id = s.Id,
                    Price = s.Price,
                    ShipmentStatus = s.ShipmentStatus,
                    OrderedAt= s.CreatedOn,
                    ReadyForPickupAt = s.ReadyForPickupAt,
                    TripStartedAt = s.TripStartedAt,
                    OutForDeliveryAt = s.OutForDeliveryAt,
                    DeliveredAt = s.DeliveredAt,

                    CustomerName = s.Customer.ApplicationUser.FirstName + " " + s.Customer.ApplicationUser.LastName,
                    CustomerPhoneNumber = s.Customer.ApplicationUser.PhoneNumber,

                    DeliveryAddress =s.DeliveryAddress,
 
                    Orders = s.Orders.Select(o => new OrderSummaryDTO
                    {
                        OrderId = o.Id,
                        StoreName = o.Seller.Name,
                        ItemsCount = o.FixedProductItems.Count() 
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (shipment == null)
            {
                return Result.Failure<GetDriverShipmentByIdResponseDTO>(ShipmentErrors.NotFound);
            }
            return Result.Success(shipment);
        }
    }
}

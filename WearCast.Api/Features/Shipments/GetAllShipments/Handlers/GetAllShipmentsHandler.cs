using WearCast.Api.Features.Shipments.GetAllShipments.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Shipments.GetAllShipments.Handlers
{
    public class GetAllShipmentsHandler : IRequestHandler<GetAllShipmentsRequestDTO, Result<IEnumerable<GetAllShipmentsResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllShipmentsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<GetAllShipmentsResponseDTO>>> Handle(
            GetAllShipmentsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var shipments = await _context.Shipments
                .AsNoTracking()
                .Select(s => new GetAllShipmentsResponseDTO
                {
                    Id = s.Id,
                    OrderTime = s.CreatedOn,
                    Price = s.Price,
                    ShipmentStatus = s.ShipmentStatus,

                    CustomerName = s.Customer.ApplicationUser.FirstName + " " + s.Customer.ApplicationUser.LastName,

                    CustomerPhoneNumber = s.Customer.ApplicationUser.PhoneNumber,

                    DriverName = s.Driver != null ?
                        s.Driver.ApplicationUser.FirstName + " " + s.Driver.ApplicationUser.LastName : null,

                    DeliveryCity = s.DeliveryAddress.City
                })
                .ToListAsync(cancellationToken);

            return Result.Success(shipments.AsEnumerable());
        }
    }
}
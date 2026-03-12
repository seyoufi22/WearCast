using WearCast.Api.Features.Shipments.GetAllShipments.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Shipments.GetAllShipments.Handlers
{
    public class GetAllShipmentsHandler : IRequestHandler<GetAllShipmentsRequestDTO, IEnumerable<GetAllShipmentsResponseDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllShipmentsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllShipmentsResponseDTO>> Handle(
            GetAllShipmentsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var shipments = await _context.Shipments
                .Select(s => new GetAllShipmentsResponseDTO
                {
                    Id = s.Id,
                    OrderTime = s.CreatedOn,
                    Price = s.Price,
                    ShipmentStatus = s.ShipmentStatus,

                    CustomerName = s.Customer != null ? s.Customer.ApplicationUser.FirstName + " " + s.Customer.ApplicationUser.LastName : null,
                    CustomerEmail = s.Customer != null ? s.Customer.ApplicationUser.Email : null,
                    CustomerPhoneNumber = s.Customer != null ? s.Customer.ApplicationUser.PhoneNumber : null,

                    DriverName = s.Driver != null && s.Driver.ApplicationUser != null && !s.Driver.ApplicationUser.IsDisabled
                        ? s.Driver.ApplicationUser.FirstName + " " + s.Driver.ApplicationUser.LastName
                        : null,

                    PickUpCity = s.PickUpAddress.City,
                    DeliveryCity = s.DeliveryAddress.City
                })
                .ToListAsync(cancellationToken);

            return shipments;
        }
    }
}
using WearCast.Api.Features.AssignShipment.DTOs;

namespace WearCast.Api.Features.AssignShipment.Handlers
{
    public class AssignShipmentHandler : IRequestHandler<AssignShipmentRequestDTO, AssignShipmentResponseDTO>
    {
        private readonly ApplicationDbContext _context;

        public AssignShipmentHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AssignShipmentResponseDTO> Handle(
            AssignShipmentRequestDTO request,
            CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

            if (shipment == null)
            {
                return new AssignShipmentResponseDTO
                {
                    IsSuccess = false,
                    Message = "Shipment not found"
                };
            }

            var driver = await _context.Drivers
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);

            if (driver == null)
            {
                return new AssignShipmentResponseDTO
                {
                    IsSuccess = false,
                    Message = "Driver not found"
                };
            }

            if (driver.ApplicationUser != null && driver.ApplicationUser.IsDisabled)
            {
                return new AssignShipmentResponseDTO
                {
                    IsSuccess = false,
                    Message = "This driver is disabled"
                };
            }

            if (driver.Status == DriverStatus.NotAvailable)
            {
                return new AssignShipmentResponseDTO
                {
                    IsSuccess = false,
                    Message = "This driver is not available"
                };
            }

            if (shipment.ShipmentStatus != ShipmentStatus.UnAssigned)
            {
                return new AssignShipmentResponseDTO
                {
                    IsSuccess = false,
                    Message = "This Shipment is assigned already"
                };
            }

            shipment.DriverId = request.DriverId;
            shipment.ShipmentStatus = ShipmentStatus.Assigned;

            await _context.SaveChangesAsync(cancellationToken);

            return new AssignShipmentResponseDTO
            {
                IsSuccess = true,
                Message = "The Shipment is assigned successfully"
            };
        }
    }
}

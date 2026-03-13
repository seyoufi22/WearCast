using System.Security.Claims;
using WearCast.Api.Features.UpdateShipmentStatus.DTOs;

namespace WearCast.Api.Features.UpdateShipmentStatus.Handler
{
    public class UpdateShipmentStatusHandler : IRequestHandler<UpdateShipmentStatusRequestDTO, UpdateShipmentStatusResponseDTO>
    {
        private readonly ApplicationDbContext _context;

        public UpdateShipmentStatusHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateShipmentStatusResponseDTO> Handle(
            UpdateShipmentStatusRequestDTO request,
            CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .Include(s => s.Driver)
                .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

            if (shipment == null)
            {
                return new UpdateShipmentStatusResponseDTO { IsSuccess = false, Message = "Shipment not found" };
            }

            //bool isAdmin = request.UserRole == "Admin" ;
            //var U=User.F
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool isAdmin = true;
            bool isAssignedDriver = shipment.Driver != null && shipment.Driver.UserId == request.UserId;

            if (!isAdmin && !isAssignedDriver)
            {
                return new UpdateShipmentStatusResponseDTO
                {
                    IsSuccess = false,
                    Message = "Not authorized"
                };
            }
           
            if (shipment.ShipmentStatus == WearCast.Api.Common.Enums.ShipmentStatus.Delivered)
            {
                return new UpdateShipmentStatusResponseDTO
                {
                    IsSuccess = false,
                    Message = "This shipment is already Delivered"
                };
            }

            shipment.ShipmentStatus = request.NewStatus;
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateShipmentStatusResponseDTO
            {
                IsSuccess = true,
                Message = "Shipment status updated successfully"
            };
        }
    }
}

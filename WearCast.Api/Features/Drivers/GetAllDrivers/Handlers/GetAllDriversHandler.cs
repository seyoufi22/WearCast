using System.Security.Claims;
using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Drivers.GetAllDrivers.DTOs;

namespace WearCast.Api.Features.Drivers.GetAllDrivers.Handlers
{
    public class GetAllDriversHandler : IRequestHandler<GetAllDriversRequestDTO, Result<PagingViewModel<GetAllDriversResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllDriversHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagingViewModel<GetAllDriversResponseDTO>>> Handle(
            GetAllDriversRequestDTO request,
            CancellationToken cancellationToken)
        {
            var query = _context.Drivers.AsNoTracking();
            if (request.IsDeleted.HasValue)
            {
                query = query.Where(d => d.IsDeleted == request.IsDeleted);
            }
            if (!string.IsNullOrWhiteSpace(request.DriverCity))
            {
                query = query.Where(d => d.Address.City.Contains(request.DriverCity.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.DriverFirstName))
            {
                query = query.Where(d => d.ApplicationUser.FirstName.Contains(request.DriverFirstName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.DriverLastName))
            {
                query = query.Where(d => d.ApplicationUser.LastName.Contains(request.DriverLastName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.DriverNationalId))
            {
                query = query.Where(d => d.NationalId == request.DriverNationalId);
            }
            if (request.DriverStatus.HasValue)
            {
                query = query.Where(d=>d.Status == request.DriverStatus);
            }
            if (request.VehicleType.HasValue)
            {
                query = query.Where(d => d.VehicleType == request.VehicleType);
            }
            query = request.SortBy switch
            {
                SortBy.NumberOfAssignedShipmentsAsc => query.OrderBy(d => d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.Assigned)),
                SortBy.NumberOfAssignedShipmentsDesc => query.OrderByDescending(d => d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.Assigned)),
                SortBy.NumberOfDeliveredShipmentsAsc => query.OrderBy(d => d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.Delivered)),
                SortBy.NumberOfDeliveredShipmentsDesc => query.OrderByDescending(d => d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.Delivered)),
                SortBy.NumberOfActiveShipmentsAsc => query.OrderBy(d => d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.PickingUp || s.ShipmentStatus == ShipmentStatus.OutForDelivery)),
                SortBy.NumberOfActiveShipmentsDesc => query.OrderByDescending(d => d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.PickingUp || s.ShipmentStatus == ShipmentStatus.OutForDelivery)),

                _ => query.OrderBy(d => d.Id)
            };
            query = query.Where(d => d.ApplicationUser.EmailConfirmed);
            var driverssquery = query
                .Select(d => new GetAllDriversResponseDTO
                {
                    Id = d.Id,
                    DriverName = d.ApplicationUser!.FirstName + " " + d.ApplicationUser.LastName,
                    VehicleType = d.VehicleType,
                    Status = d.Status,
                    DriverCity = d.Address.City,
                    IsDeleted = d.IsDeleted,
                    NumberOfAssignedShipments = d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.Assigned),
                    NumberOfActiveShipments = d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.PickingUp || s.ShipmentStatus == ShipmentStatus.OutForDelivery),
                    NumberOfDeliveredShipments = d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.Delivered),
                    ProfileImageUrl = d.ProfileImageUrl
                });
            var pagedResult = await PagingHelper.CreateAsync(driverssquery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}
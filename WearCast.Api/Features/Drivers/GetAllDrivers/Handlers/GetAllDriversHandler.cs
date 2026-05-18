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
            var query = _context.Drivers.AsNoTracking()
                .Where(d => d.ApplicationUser.EmailConfirmed);

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
                query = query.Where(d => d.Status == request.DriverStatus);
            }
            if (request.VehicleType.HasValue)
            {
                query = query.Where(d => d.VehicleType == request.VehicleType);
            }

            var projectedQuery = query.Select(d => new GetAllDriversResponseDTO
            {
                Id = d.Id,
                DriverName = d.ApplicationUser!.FirstName + " " + d.ApplicationUser.LastName,
                VehicleType = d.VehicleType,
                Status = d.Status,
                DriverCity = d.Address.City,
                NumberOfAssignedShipments = d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.Assigned),
                NumberOfActiveShipments = d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.PickingUp || s.ShipmentStatus == ShipmentStatus.OutForDelivery),
                NumberOfDeliveredShipments = d.Shipments.Count(s => s.ShipmentStatus == ShipmentStatus.Delivered)
            });

            projectedQuery = request.SortBy switch
            {
                SortBy.NumberOfAssignedShipmentsAsc => projectedQuery.OrderBy(d => d.NumberOfAssignedShipments),
                SortBy.NumberOfAssignedShipmentsDesc => projectedQuery.OrderByDescending(d => d.NumberOfAssignedShipments),
                SortBy.NumberOfDeliveredShipmentsAsc => projectedQuery.OrderBy(d => d.NumberOfDeliveredShipments),
                SortBy.NumberOfDeliveredShipmentsDesc => projectedQuery.OrderByDescending(d => d.NumberOfDeliveredShipments),
                SortBy.NumberOfActiveShipmentsAsc => projectedQuery.OrderBy(d => d.NumberOfActiveShipments),
                SortBy.NumberOfActiveShipmentsDesc => projectedQuery.OrderByDescending(d => d.NumberOfActiveShipments),
                _ => projectedQuery.OrderByDescending(d => d.Id)
            };

            var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}
using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Shipments.AdminAndManager.GetAllShipments.DTOs;
using WearCast.Api.Features.Shipments.Customer.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Shipments.AdminAndManager.GetAllShipments.Handlers
{
    public class GetAllShipmentsHandler : IRequestHandler<GetAllShipmentsRequestDTO, Result<PagingViewModel<GetAllShipmentsResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllShipmentsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagingViewModel<GetAllShipmentsResponseDTO>>> Handle(
            GetAllShipmentsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var query = _context.Shipments.AsNoTracking();

            if (request.ShipmentStatus.HasValue)
            {
                query = query.Where(s => s.ShipmentStatus == request.ShipmentStatus);
            }
            if (!string.IsNullOrWhiteSpace(request.DeliveryState))
            {
                query = query.Where(s => s.DeliveryAddress.State.Contains(request.DeliveryState.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.DeliveryCity))
            {
                query = query.Where(s => s.DeliveryAddress.City.Contains(request.DeliveryCity.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.DeliveryStreet))
            {
                query = query.Where(s => s.DeliveryAddress.Street.Contains(request.DeliveryStreet.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.DriverFirstName))
            {
                query = query.Where(s => s.Driver != null && s.Driver.ApplicationUser.FirstName.Contains(request.DriverFirstName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.DriverLastName))
            {
                query = query.Where(s => s.Driver != null && s.Driver.ApplicationUser.LastName.Contains(request.DriverLastName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.DriverNationalId))
            {
                query = query.Where(s => s.Driver != null && s.Driver.NationalId == request.DriverNationalId);
            }
            if (!string.IsNullOrWhiteSpace(request.CustomerFirstName))
            {
                query = query.Where(s => s.Customer.ApplicationUser.FirstName.Contains(request.CustomerFirstName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.CustomerLastName))
            {
                query = query.Where(s => s.Customer.ApplicationUser.LastName.Contains(request.CustomerLastName.Trim()));
            }
            if (request.MinPrice.HasValue)
            {
                query = query.Where(s => s.Price >= request.MinPrice);
            }
            if (request.MaxPrice.HasValue)
            {
                query = query.Where(s => s.Price <= request.MaxPrice);
            }
            if (request.MinNumberOfOrders.HasValue)
            {
                query = query.Where(s => s.Orders.Count() >= request.MinNumberOfOrders);
            }
            if (request.MaxNumberOfOrders.HasValue)
            {
                query = query.Where(s => s.Orders.Count() <= request.MaxNumberOfOrders);
            }
            if (request.StartDate.HasValue)
            {
                var start = request.StartDate.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(s => s.CreatedOn >= start);
            }
            if (request.EndDate.HasValue)
            {
                var end = request.EndDate.Value.ToDateTime(TimeOnly.MinValue).AddDays(1);
                query = query.Where(s => s.CreatedOn < end);
            }
            query = request.SortBy switch
            {
                SortBy.Oldest => query.OrderBy(s => s.CreatedOn),
                SortBy.NumberOfOrdersAsc => query.OrderBy(s => s.Orders.Count()),
                SortBy.NumberOfOrdersDesc => query.OrderByDescending(s => s.Orders.Count()),
                SortBy.PriceAsc => query.OrderBy(s => s.Price),
                SortBy.PriceDesc => query.OrderByDescending(s => s.Price),
                _ => query.OrderByDescending(s => s.CreatedOn)
            };
            var shipmentsquery = query
               .Select(s => new GetAllShipmentsResponseDTO
               {
                   Id = s.Id,
                   OrderTime = s.CreatedOn,
                   ShipmentStatus = s.ShipmentStatus,
                   Price = s.Price,
                   NumberOfOrders = s.Orders.Count(),
                   DeliveryState = s.DeliveryAddress.State,
                   DeliveryCity = s.DeliveryAddress.City,
                   DeliveryStreet = s.DeliveryAddress.Street,
                   DeliveryCode = s.DeliveryCode,
                   DriverName = s.Driver == null ? null : s.Driver.ApplicationUser.FirstName + " " + s.Driver.ApplicationUser.LastName,
                   CustomerName = s.Customer.ApplicationUser.FirstName + " " + s.Customer.ApplicationUser.LastName
               });
            var pagedResult = await PagingHelper.CreateAsync(shipmentsquery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Shipments.Driver.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.GetAllShipments.Handlers
{
    public class GetAllDriverShipmentsHandler : IRequestHandler<GetAllDriverShipmentsRequestDTO, Result<PagingViewModel<GetAllDriverShipmentsResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllDriverShipmentsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagingViewModel<GetAllDriverShipmentsResponseDTO>>> Handle(
            GetAllDriverShipmentsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var query = _context.Shipments.AsNoTracking();
            query = query.Where(s => s.DriverId == request.DriverId && !s.IsDeleted);
            if (request.ShipmentStatus.HasValue)
            {
                query = query.Where(s => s.ShipmentStatus == request.ShipmentStatus);
            }
            if (!string.IsNullOrWhiteSpace(request.DeliveryCity))
            {
                query = query.Where(s => s.DeliveryAddress.City.Contains(request.DeliveryCity.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.DeliveryStreet))
            {
                query = query.Where(s => s.DeliveryAddress.Street.Contains(request.DeliveryStreet.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.CustomerFirstName))
            {
                query = query.Where(s => s.Customer.ApplicationUser.FirstName.Contains(request.CustomerFirstName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.CustomerLastName))
            {
                query = query.Where(s => s.Customer.ApplicationUser.LastName.Contains(request.CustomerLastName.Trim()));
            }
            query = request.SortBy switch
            {
                SortBy.Oldest => query.OrderBy(s => s.CreatedOn),
                SortBy.NumberOfOrdersAsc => query.OrderBy(s => s.Orders.Count()),
                SortBy.NumberOfOrdersDesc => query.OrderByDescending(s => s.Orders.Count()),
                _ => query.OrderByDescending(s => s.CreatedOn)
            };

            var shipmentsquery = query
                .Select(s => new GetAllDriverShipmentsResponseDTO
                {
                    Id = s.Id,
                    OrderTime = s.CreatedOn,
                    ShipmentStatus = s.ShipmentStatus,

                    CustomerName = s.Customer.ApplicationUser.FirstName + " " + s.Customer.ApplicationUser.LastName,

                    CustomerPhoneNumber = s.Customer.ApplicationUser.PhoneNumber,

                    NumberOfOrders = s.Orders.Count(),

                    DeliveryCity = s.DeliveryAddress.City,
                    DeliveryStreet = s.DeliveryAddress.Street
                });
            var pagedResult = await PagingHelper.CreateAsync(shipmentsquery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}
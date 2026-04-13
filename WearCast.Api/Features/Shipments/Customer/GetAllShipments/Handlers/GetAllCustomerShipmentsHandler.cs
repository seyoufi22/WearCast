using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Shipments.Customer.GetAllShipments.DTOs;
using WearCast.Api.Features.Shipments.Driver.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Shipments.Customer.GetAllShipments.Handlers
{
    public class GetAllCustomerShipmentsHandler : IRequestHandler<GetAllCustomerShipmentsRequestDTO, Result<PagingViewModel<GetAllCustomerShipmentsResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllCustomerShipmentsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagingViewModel<GetAllCustomerShipmentsResponseDTO>>> Handle(
            GetAllCustomerShipmentsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var query = _context.Shipments.AsNoTracking();
            query = query.Where(s => s.CustomerId == request.CustomerId && !s.IsDeleted);
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
            if (request.MinPrice.HasValue)
            {
                query=query.Where(s=>s.Price>=request.MinPrice);
            }
            if (request.MaxPrice.HasValue)
            {
                query=query.Where(s=>s.Price<=request.MaxPrice);
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
                .Select(s => new GetAllCustomerShipmentsResponseDTO
                {
                    Id = s.Id,
                    OrderTime = s.CreatedOn,
                    ShipmentStatus = s.ShipmentStatus,
                    Price = s.Price,
                    NumberOfOrders = s.Orders.Count(),

                    DeliveryCity = s.DeliveryAddress.City,
                    DeliveryStreet = s.DeliveryAddress.Street,
                    DeliveryCode=s.DeliveryCode
                });
            var pagedResult = await PagingHelper.CreateAsync(shipmentsquery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}
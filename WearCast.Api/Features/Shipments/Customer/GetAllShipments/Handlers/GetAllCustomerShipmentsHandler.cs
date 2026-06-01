using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Shipments.Customer.GetAllShipments.DTOs;

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

            var projectedQuery = query.Select(s => new GetAllCustomerShipmentsResponseDTO
            {
                Id = s.Id,
                OrderTime = s.CreatedOn,
                ShipmentStatus = s.ShipmentStatus,
                Price = s.Price,
                NumberOfOrders = s.Orders.Count(), 
                DeliveryCity = s.DeliveryAddress.City,
                DeliveryStreet = s.DeliveryAddress.Street,
                DeliveryCode = s.DeliveryCode
            });

            projectedQuery = request.SortBy switch
            {
                SortBy.Oldest => projectedQuery.OrderBy(s => s.OrderTime),
                SortBy.NumberOfOrdersAsc => projectedQuery.OrderBy(s => s.NumberOfOrders),
                SortBy.NumberOfOrdersDesc => projectedQuery.OrderByDescending(s => s.NumberOfOrders),
                SortBy.PriceAsc => projectedQuery.OrderBy(s => s.Price),
                SortBy.PriceDesc => projectedQuery.OrderByDescending(s => s.Price),
                _ => projectedQuery.OrderByDescending(s => s.OrderTime)
            };

            var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}
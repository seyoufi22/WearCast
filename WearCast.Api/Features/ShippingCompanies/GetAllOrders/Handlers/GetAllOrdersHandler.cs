using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.ShippingCompanies.GetAllOrders.DTOs;

namespace WearCast.Api.Features.ShippingCompanies.GetAllOrders.Handlers
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersRequestDTO, Result<PagingViewModel<GetAllOrdersResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllOrdersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagingViewModel<GetAllOrdersResponseDTO>>> Handle(
            GetAllOrdersRequestDTO request,
            CancellationToken cancellationToken)
        {
            var query = _context.Orders.AsNoTracking();

            if (request.OrderStatus.HasValue)
            {
                query = query.Where(o => o.Status == request.OrderStatus.Value);
            }

            if (request.OrderType.HasValue)
            {
                query = request.OrderType.Value == OrderType.Fixed
                    ? query.Where(o => o.SellerId.HasValue)
                    : query.Where(o => !o.SellerId.HasValue);
            }

            if (!string.IsNullOrWhiteSpace(request.VendorCity))
            {
                var cityTrimmed = request.VendorCity.Trim();
                query = query.Where(o =>
                    (o.Seller != null && o.Seller.Address.City.Contains(cityTrimmed)) ||
                    (o.Factory != null && o.Factory.Address.City.Contains(cityTrimmed))
                );
            }

            if (request.ShipmentStatus.HasValue)
            {
                query = query.Where(o => o.Shipment != null && o.Shipment.ShipmentStatus == request.ShipmentStatus.Value);
            }

            var projectedQuery = query.Select(o => new GetAllOrdersResponseDTO
            {
                OrderId = o.Id,
                ShipmentId = o.ShipmentId ?? 0,
                OrderType = o.SellerId.HasValue ? OrderType.Fixed : OrderType.Designed,

                VendorName = o.Seller != null ? o.Seller.Name : (o.Factory != null ? o.Factory.Name : string.Empty),
                VendorPhoneNumber = o.Seller != null ? o.Seller.PhoneNumber : (o.Factory != null ? o.Factory.PhoneNumber : string.Empty),
                VendorAddress = o.Seller != null ? o.Seller.Address : (o.Factory != null ? o.Factory.Address : new Address()),

                OrderStatus = o.Status,
                NumberOfItems = o.FixedProductItems.Count + o.DesignedProductItems.Count,

                ShipmentStatus = o.Shipment != null ? o.Shipment.ShipmentStatus : ShipmentStatus.Pending,
                CreatedOn = o.CreatedOn 
            });

            projectedQuery = request.SortBy switch
            {
                SortBy.NumberOfItemsAsc => projectedQuery.OrderBy(o => o.NumberOfItems),
                SortBy.NumberOfItemsDesc => projectedQuery.OrderByDescending(o => o.NumberOfItems),
                SortBy.Oldest => projectedQuery.OrderBy(o => o.CreatedOn),
                _ => projectedQuery.OrderByDescending(o => o.CreatedOn)
            };

            var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}


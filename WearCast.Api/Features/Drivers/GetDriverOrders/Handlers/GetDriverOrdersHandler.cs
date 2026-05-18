using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Drivers.GetDriverOrders.DTOs;

namespace WearCast.Api.Features.Drivers.GetDriverOrders.Handlers
{
    public class GetDriverOrdersHandler : IRequestHandler<GetDriverOrdersRequestDTO, Result<PagingViewModel<GetDriverOrdersResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;

        public GetDriverOrdersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagingViewModel<GetDriverOrdersResponseDTO>>> Handle(
            GetDriverOrdersRequestDTO request,
            CancellationToken cancellationToken)
        {
            
            var query = _context.Orders
                .Where(o => o.Shipment != null && o.Shipment.DriverId == request.DriverId)
                .AsNoTracking();

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

            query = request.SortBy switch
            {
                SortBy.NumberOfItemsAsc => query.OrderBy(o => o.DesignedProductItems.Count + o.FixedProductItems.Count),
                SortBy.NumberOfItemsDesc => query.OrderByDescending(o => o.DesignedProductItems.Count + o.FixedProductItems.Count),
                SortBy.Oldest => query.OrderBy(o => o.CreatedOn),
                _ => query.OrderByDescending(o => o.CreatedOn)
            };

            var projectedQuery = query.Select(o => new GetDriverOrdersResponseDTO
            {
                OrderId = o.Id,
                ShipmentId = o.ShipmentId ?? 0,
                OrderType = o.SellerId.HasValue ? OrderType.Fixed : OrderType.Designed,

                VendorName = o.Seller != null ? o.Seller.Name : (o.Factory != null ? o.Factory.Name : string.Empty),
                VendorPhoneNumber = o.Seller != null ? o.Seller.PhoneNumber : (o.Factory != null ? o.Factory.PhoneNumber : string.Empty),
                VendorAddress = o.Seller != null ? o.Seller.Address : (o.Factory != null ? o.Factory.Address : new Address()),

                OrderStatus = o.Status,
                NumberOfItems = o.FixedProductItems.Count + o.DesignedProductItems.Count
            });

            var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}
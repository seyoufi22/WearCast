using WearCast.Api.Features.Sellers.SellerManagers.GetSellerManager;

namespace WearCast.Api.Features.Sellers.SellerManagers.GetAllSellerManagers;

public record GetAllSellerManagersRequest(
    int? ProvidedSellerId = null
) : IRequest<Result<List<GetSellerManagerResponse>>>;
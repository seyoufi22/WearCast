namespace WearCast.Api.Features.Sellers.SellerManagers.GetSellerManager;

public record GetSellerManagerRequest(
    int? ProvidedManagerId = null
) : IRequest<Result<GetSellerManagerResponse>>;
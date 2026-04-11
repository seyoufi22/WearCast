namespace WearCast.Api.Features.Sellers.GetSeller;

public record GetSellerRequest(
    int? ProvidedSellerId = null
) : IRequest<Result<GetSellerResponse>>;
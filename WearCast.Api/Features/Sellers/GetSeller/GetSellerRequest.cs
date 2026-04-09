namespace WearCast.Api.Features.Sellers.GetSeller;

public record GetSellerRequest(int Id) : IRequest<Result<GetSellerResponse>>;

namespace WearCast.Api.Features.Sellers.DeleteSeller;

public record DeleteSellerRequest(
    int SellerId,
    string Reason 
) : IRequest<Result>;
public record DeleteSellerBody(
    string Reason
);
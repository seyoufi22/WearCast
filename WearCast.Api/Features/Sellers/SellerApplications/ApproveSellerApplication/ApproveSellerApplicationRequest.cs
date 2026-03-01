namespace WearCast.Api.Features.Sellers.SellerApplications.ApproveSellerApplication
{
    public record ApproveSellerApplicationRequest(
        string Email
        ) : IRequest<Result>;
}

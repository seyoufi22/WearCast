namespace WearCast.Api.Features.Sellers.SellerApplications.RejectSellerApplication
{
    public record RejectSellerApplicationRequest(
        string Email,
        string RejectionReason
        ) : IRequest<Result>;
}

namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.RejectSellerApplication
{
    public record RejectSellerApplicationRequest(
        string Email,
        string RejectionReason
        ) : IRequest<Result>;
}

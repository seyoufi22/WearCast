namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ApproveSellerApplication
{
    public record ApproveSellerApplicationRequest(
        string Email
        ) : IRequest<Result>;
}

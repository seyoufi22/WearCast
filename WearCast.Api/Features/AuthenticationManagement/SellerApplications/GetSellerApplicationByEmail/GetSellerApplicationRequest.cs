namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.GetSellerApplicationByEmail
{
    public record GetSellerApplicationRequest(
        string Email
        ) : IRequest<Result<SellerApplicationResponse>>;
}

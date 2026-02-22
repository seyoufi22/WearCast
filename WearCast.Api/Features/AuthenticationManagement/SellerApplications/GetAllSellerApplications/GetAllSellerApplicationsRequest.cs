namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.GetAllSellerApplications
{
    public record GetSellerApplicationRequest() : IRequest<Result<IEnumerable<SellerApplicationResponse>>>;
}

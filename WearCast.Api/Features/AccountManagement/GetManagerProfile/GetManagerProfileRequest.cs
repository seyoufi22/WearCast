namespace WearCast.Api.Features.AccountManagement.GetManagerProfile;

public record GetManagerProfileRequest(string Id) : IRequest<Result<GetManagerProfileResponse>>;

namespace WearCast.Api.Features.AccountManagement.GetManagerProfile;

public record GetManagerProfileResponse
(
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? Email
);

namespace WearCast.Api.Features.Drivers.CreateDriver
{
    public record CreateDriverRequest(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Password,
        string ConfirmPassword,


        IFormFile ProfileImage,
        string NationalId,
        DeliveryVehicleType VehicleType,
        string? VehiclePlateNumber,

        string State,
        string City,
        string Street,
        string BuildingNumber
        ) : IRequest<Result<CreateDriverResponse>>;
}
